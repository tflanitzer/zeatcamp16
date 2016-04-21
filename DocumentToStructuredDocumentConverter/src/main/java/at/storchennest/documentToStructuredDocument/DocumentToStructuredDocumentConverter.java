package at.storchennest.documentToStructuredDocument;

import java.util.ArrayList;
import java.util.Calendar;
import java.util.GregorianCalendar;
import java.util.HashMap;
import java.util.List;

import org.bson.Document;
import org.bson.types.ObjectId;

import com.mongodb.client.FindIterable;

import at.storchennest.documentToStructuredDocument.Database.MongoDBConnector;
import at.storchennest.documentToStructuredDocument.Database.MongoDBConnectorMorphium;
import at.storchennest.documentToStructuredDocument.Input.Converter.JSON2MailConverter;
import at.storchennest.documentToStructuredDocument.output.data.EMailAccount;
import at.storchennest.documentToStructuredDocument.output.data.Mail;
import at.storchennest.documentToStructuredDocument.output.data.Receipient;
import at.storchennest.test.data.Address;
import at.storchennest.test.data.Person;

public class DocumentToStructuredDocumentConverter {

	private static long before;
	
//	public static void main(String[] args) {
//		System.out.println("Hello World");
//		MongoDBConnector mdbConnectorReadDB = new MongoDBConnector("mongodb://104.46.32.71:27017","test");
//		mdbConnectorReadDB.connectToMongoDB();
//
//		MongoDBConnector mdbConnectorWriteDB = new MongoDBConnector("mongodb://104.46.32.71:27017","sNoSql");
//		mdbConnectorWriteDB.connectToMongoDB();
//
//		JSON2MailConverter converter = new JSON2MailConverter("From","To","Cc","Bcc","Subject","body");
//		HashMap<String,EMailAccount> accountCache = new HashMap<>();
//		FindIterable<Document> emailDocumentList = mdbConnectorWriteDB.readOutDocumentsFromCollection("EMailAccount");
//		for(Document document: emailDocumentList){
//			EMailAccount account = new EMailAccount(document.getString(EMailAccount.P_MAIL_ADDRESS_FIELD));
//			account.setObjectID(document.getObjectId("_id"));
//			accountCache.put(document.getString(EMailAccount.P_MAIL_ADDRESS_FIELD), account);
//		}
//
//		
//		System.out.println("Start reading out documents");
//		FindIterable<Document> documentList = mdbConnectorReadDB.readOutDocumentsFromCollection("messages");
//
//		int mailCounter =0;
//		System.out.println("Start processing");
//		for(Document document: documentList){
//			Mail mail = converter.convertDocumentToMail(document);
//		
//			EMailAccount sender = mail.getSender();
//			if(sender!=null){
//				if(accountCache.containsKey(sender.getMailAddress())){
//					sender.setObjectID(accountCache.get(sender.getMailAddress()).getObjectID());
//				}else{
//					accountCache.put(sender.getMailAddress(), sender);
//					ObjectId objectId = mdbConnectorWriteDB.insert(sender.createDocument(), "EMailAccount");
//					sender.setObjectID(objectId);
//				}
//
//			}
//			List<Receipient> receipients = mail.getReceipients();
//			
//			for(Receipient receipient : receipients){
//				EMailAccount receipientAccount = receipient.getEmailAccount();
//				if(accountCache.containsKey(receipientAccount.getMailAddress())){
//					receipientAccount.setObjectID(accountCache.get(receipientAccount.getMailAddress()).getObjectID());
//				}else{
//					accountCache.put(receipientAccount.getMailAddress(), receipientAccount);
//					ObjectId objectId = mdbConnectorWriteDB.insert(receipientAccount.createDocument(), "EMailAccount");
//					receipientAccount.setObjectID(objectId);
//				}
//			}
//			mdbConnectorWriteDB.cachedInsert(mail.createDocument(), "mail");
//
//			
//			if(mailCounter == 10000){
//				System.out.println("Another 10000 mails processed");
//				mailCounter=0;
//			}else{
//				mailCounter++;
//			}
//		}
//		System.out.println("Procesing finished");
//
//     	mdbConnectorReadDB.closeDatabase();
//		mdbConnectorWriteDB.closeDatabase();
//		
//
//		
//		
//	}
	
	// fatMail
	
	
	public static void main(String[] args) {
		MongoDBConnector mdbConnectorReadDB = new MongoDBConnector("mongodb://104.46.32.71:27017","test");
		mdbConnectorReadDB.connectToMongoDB();

		MongoDBConnector mdbConnectorWriteDB = new MongoDBConnector("mongodb://104.46.32.71:27017","sNoSql");
		mdbConnectorWriteDB.connectToMongoDB();

		JSON2MailConverter converter = new JSON2MailConverter("From","To","Cc","Bcc","Subject","body","_id","Date");

		
		System.out.println("Start reading out documents");
		FindIterable<Document> documentList = mdbConnectorReadDB.readOutDocumentsFromCollection("messages");

		int mailCounter =0;
		System.out.println("Start processing");
		for(Document document: documentList){
			Mail mail = converter.convertDocumentToMail(document);
		
			//mdbConnectorWriteDB.cachedInsert(mail.createFatDocument(), "fatMail");

			
			if(mailCounter == 10000){
				System.out.println("Another 10000 mails processed");
				mailCounter=0;
			}else{
				mailCounter++;
			}
		}
		System.out.println("Procesing finished");

     	mdbConnectorReadDB.closeDatabase();
		mdbConnectorWriteDB.closeDatabase();
				
		
	}
	
//	public static void main(String[] args) {
//		MongoDBConnectorMorphium connector = new MongoDBConnectorMorphium();
//		Person a=  new Person("Lukas",GregorianCalendar.getInstance(),1578);
//		
////		a.setAddress(new Address("Gra",2440,"MueWeg"));
//		
//		connector.writeToCollection("persons", a);
//		connector.close();
//		
//		
//
//		
//		
//	}
	public static void startTime(){
		before = Calendar.getInstance().getTimeInMillis();
	}
	
	public static String getDuration(){
		long duration =Calendar.getInstance().getTimeInMillis()-before;
		return duration + "ms";
	}

}
