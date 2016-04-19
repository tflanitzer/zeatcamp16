#!/usr/bin/env python

# 1) Reads from MongoDB
# 2) KMeans clustering of tf/idf vectors of mails
# 3) Store in MongoDB

from __future__ import print_function

from pymongo import MongoClient
from sklearn.feature_extraction import DictVectorizer


from sklearn.datasets import fetch_20newsgroups
from sklearn.decomposition import TruncatedSVD
from sklearn.feature_extraction.text import TfidfVectorizer
from sklearn.feature_extraction.text import HashingVectorizer
from sklearn.feature_extraction.text import TfidfTransformer
from sklearn.pipeline import make_pipeline
from sklearn.preprocessing import Normalizer
from sklearn import metrics

from sklearn.cluster import KMeans, MiniBatchKMeans
from time import time

import numpy as np
import sys



if len(sys.argv) < 2:
    print("USAGE: %s <number of records to load>" % sys.argv[0])
    exit (1)



nRecs = int(sys.argv[1])

ar=[None]*nRecs
ar_dict=[None]*nRecs

print("Loading data from MongoDB : %d records" % nRecs)
t0 = time()


client = MongoClient('localhost', 27017)
db = client['test']
msgs = db['messages']

dbout = client['sNoSql']
clusters = dbout['clusters']
cluster_terms = dbout['cluster_terms']



## Insert clusters
#print ('Records in clusters collection: %d. ' % int(clusters.count()))
#result = clusters.insert_many([{'_id': '12', 'terms': term} for term in terms])
#result = clusters.insert_many([{'_id': i} for i in range(2)])
#print (result.inserted_ids)
#print ('Records in clusters collection: %d. ' % int(clusters.count()))
#exit(4)







cursor = msgs.find(limit=nRecs)

#print len(cursor)
i = 0
for entry in cursor:
#    ar = ar + [ entry['body'] ]
    ar[i] = entry['body']
#    ar = ar + [ {'id': 'asdf', 'body': entry['body']  } ]
#    dict[entry['_id']] = entry['body']
#    ar_dict = ar_dict + [{'_id' : entry['_id'], 'body': entry['body']}]
    ar_dict[i] = {'_id' : entry['_id'], 'body': entry['body']}
    #print(entry)
    #for e in entry:
    #    print (e)
    i += 1

#print (len(dict))
ar_len = i
print ("%d records loaded" % ar_len)

ar = ar[:ar_len-1]
ar_dict = ar_dict[:ar_len-1]


print("done in %0.3fs" % (time() - t0))
print()

#for entry in dict.keys():
#    print (entry)
#for entry in dict.items():
#    print entry



#print vec.fit_transform(ar).toarray()
#print vec.get_feature_names()


# opts

true_k = 4


print("Extracting features from the training dataset using a sparse vectorizer")
t0 = time()

vectorizer = TfidfVectorizer(max_df=0.5,
                                 min_df=2, stop_words='english',
#                                 min_df=2, stop_words=['http', 'www'],
                                 use_idf=1)
# use array
X = vectorizer.fit_transform(ar)
# use dict
#X = vectorizer.fit_transform(dict)
# use array of dicts
#X = vectorizer.fit_transform(ar_dict)


print("done in %fs" % (time() - t0))
print("n_samples: %d, n_features: %d" % X.shape)
#print (X)
print()


km = KMeans(n_clusters=true_k, init='k-means++', max_iter=100, n_init=3,
                verbose=0)


print("Clustering sparse data with %s" % km)
t0 = time()
km.fit(X)
labels=km.predict(X)
print("done in %0.3fs" % (time() - t0))
print()

#print("Homogeneity: %0.3f" % metrics.homogeneity_score(labels, km.labels_))
#print("Completeness: %0.3f" % metrics.completeness_score(labels, km.labels_))
#print("V-measure: %0.3f" % metrics.v_measure_score(labels, km.labels_))
#print("Adjusted Rand-Index: %.3f"
#      % metrics.adjusted_rand_score(labels, km.labels_))
#print("Silhouette Coefficient: %0.3f"
#      % metrics.silhouette_score(X, km.labels_, sample_size=1000))

print()



#print("labels:", end='')
#print(labels)


print("Top terms per cluster:")

order_centroids = km.cluster_centers_.argsort()[:, ::-1]

#print(km.cluster_centers_.shape)
#print(order_centroids.shape)
#print(order_centroids)
#print(km.cluster_centers_)
#print(vectorizer.get_feature_names())




## Insert clusters
print ("Writing to MongoDB collection %s" % clusters.full_name)
t0 = time()

# Remove all current records
result = clusters.delete_many({})
print ('Deleted records: %d. ' % int(result.deleted_count))
print ('Records in clusters collection: %d. ' % int(clusters.count()))
i = 0
to_insert=[]
for item in ar_dict:
    to_insert += [{'OriginalId': item['_id'], 'cluster': str(labels[i])}]
    i += 1
result = clusters.insert_many(to_insert)
#print (result.inserted_ids)
print ('Records in clusters collection: %d. ' % int(clusters.count()))
print("done in %0.3fs" % (time() - t0))
print()




## Insert clusters centers
print ("Writing to MongoDB collection %s" % cluster_terms.full_name)
t0 = time()

# Remove all current records
result = cluster_terms.delete_many({})
print ('Deleted records: %d. ' % int(result.deleted_count))

to_insert=[]
terms = vectorizer.get_feature_names()
for i in range(true_k):
    print("Cluster %d:" % i, end='')
    for ind in order_centroids[i, :10]:
        print(' %s' % terms[ind], end='')
        to_insert += [{'cluster': i, 'term': terms[ind]}]
    print()

# insert!
result = cluster_terms.insert_many(to_insert)
#print (result.inserted_ids)
print ('Records in collection: %d. ' % int(cluster_terms.count()))
print("done in %0.3fs" % (time() - t0))
print()



