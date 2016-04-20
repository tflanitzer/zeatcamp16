package at.storchennest.documentToStructuredDocument.Database;


import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.HashMap;
import java.util.List;
import java.util.Map.Entry;

import org.bson.Document;
import org.bson.conversions.Bson;
import org.bson.types.ObjectId;

import at.storchennest.documentToStructuredDocument.output.data.EMailAccount;
import at.storchennest.documentToStructuredDocument.output.data.Mail;

import com.mongodb.MongoClient;
import com.mongodb.MongoClientURI;
import com.mongodb.WriteResult;
import com.mongodb.client.FindIterable;
import com.mongodb.client.MongoCollection;
import com.mongodb.client.MongoDatabase;

import static com.mongodb.client.model.Filters.eq;

public class MongoDBConnector {
	
	private MongoDatabase database;
	private MongoClient client;
	private String connectionString;
	private String databaseID;
	private int counter=0;
	
	private HashMap<String,List<Document>> cache;
	
	
	public MongoDBConnector(String connectionString, String databaseID){
		cache=new HashMap<>();
		this.connectionString = connectionString;
		this.databaseID = databaseID;
	}
	public void connectToMongoDB(){
//		MongoClient client = new MongoClient(new MongoClientURI("mongodb://localhost:27017"));
		client = new MongoClient(new MongoClientURI(connectionString));
		database = client.getDatabase(databaseID);

	}
		
	public FindIterable<Document> readOutDocumentsFromCollection(String collection){
		MongoCollection<Document> messageCollection = database.getCollection(collection);
		FindIterable<Document> documentList =  messageCollection.find();
		return documentList;
	}
	
	public ObjectId insertOrUpdateDocument(Document document, String collectionName, String primaryKey){
		FindIterable<Document> documents = database.getCollection(collectionName).find(eq(primaryKey, document.getString(primaryKey)));
		for(Document doc : documents){
			return doc.getObjectId("_id");
		}
		database.getCollection(collectionName).insertOne(document);
		return document.getObjectId("_id");
	}
	
	
	public void cachedInsert(Document document, String collectionName){
		if(counter<100000){
			if(cache.containsKey(collectionName)){
				cache.get(collectionName).add(document);
			}
			else{
				cache.put(collectionName, new ArrayList<Document>());
				cache.get(collectionName).add(document);
			}
			counter ++;
		}
		else{
			writeCachedElements();
			counter=0;
			
		}
//		database.getCollection(collectionName).insertOne(document);
//		return document.getObjectId("_id");
	}
	
	private void writeCachedElements(){
		System.out.println("Start writing to the database "+getTime());
		int count=0;
		for(Entry<String, List<Document>> cacheEntry: cache.entrySet()){
			database.getCollection(cacheEntry.getKey()).insertMany(cacheEntry.getValue());
			count +=cacheEntry.getValue().size();
		}
		cache.clear();
		System.out.println("Cache written to the database "+count+" elements written - "+getTime());

	}
	
	public ObjectId insert(Document document, String collectionName){
		database.getCollection(collectionName).insertOne(document);
		return document.getObjectId("_id");
	}

	
	public String getTime(){
        Calendar cal = Calendar.getInstance();
        SimpleDateFormat sdf = new SimpleDateFormat("HH:mm:ss");
        return sdf.format(cal.getTime()) ;
	}
	
	public void closeDatabase(){
		writeCachedElements();		
		client.close();
	}

}
