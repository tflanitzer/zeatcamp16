import redis

from pymongo import MongoClient

r0 = redis.Redis(db=0)

client = MongoClient()
db = client.test
col = db.messages
cursor = col.find()

for entry in cursor:
    r0.rpush('json queue', entry)






