package at.storchennest.documentToStructuredDocument.Database;


import org.bson.Document;
import org.bson.conversions.Bson;
import org.bson.types.ObjectId;

import at.storchennest.documentToStructuredDocument.output.data.EMailAccount;
import at.storchennest.documentToStructuredDocument.output.data.Mail;

import com.mongodb.MongoClient;
import com.mongodb.MongoClientURI;
import com.mongodb.client.FindIterable;
import com.mongodb.client.MongoCollection;
import com.mongodb.client.MongoDatabase;
import static com.mongodb.client.model.Filters.eq;

public class MongoDBConnector {
	
	private MongoDatabase database;
	private MongoClient client;
	private String connectionString;
	private String databaseID;
	
	
	public MongoDBConnector(String connectionString, String databaseID){
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
	public ObjectId insert(Document document, String collectionName){
		database.getCollection(collectionName).insertOne(document);
		return document.getObjectId("_id");
	}

	
	public void closeDatabase(){
		client.close();
	}

}
