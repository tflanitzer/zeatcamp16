package at.storchennest.documentToStructuredDocument.Database;


import java.util.ArrayList;

import org.bson.Document;

import at.storchennest.documentToStructuredDocument.Input.Data.Mail;

import com.mongodb.MongoClient;
import com.mongodb.MongoClientURI;
import com.mongodb.client.FindIterable;
import com.mongodb.client.MongoCollection;
import com.mongodb.client.MongoDatabase;
import com.mongodb.client.MongoIterable;

public class MongoDBConnector {
	
	public void connectToMongoDB(){
		MongoClient client = new MongoClient(new MongoClientURI("mongodb://localhost:27017"));
		MongoDatabase database = client.getDatabase("test");
		MongoCollection<Document> messageCollection = database.getCollection("messages");
		FindIterable<Document> documentList =  messageCollection.find();

		ArrayList<Mail> mails = new ArrayList<Mail>();
		for(Document document: documentList){
			Mail extractedMail = new Mail(document.getString("From"),document.getString("To"),document.getString("Subject"),null,document.getString("body"));
			mails.add(extractedMail);
		}
		System.out.println("Number of EMails: "+ mails.size());
		
		client.close();
	}

}
