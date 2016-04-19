package at.storchennest.documentToStructuredDocument;

import java.util.ArrayList;
import java.util.List;

import org.bson.types.ObjectId;

import at.storchennest.documentToStructuredDocument.Database.MongoDBConnector;
import at.storchennest.documentToStructuredDocument.Input.Converter.JSON2MailConverter;
import at.storchennest.documentToStructuredDocument.output.data.EMailAccount;
import at.storchennest.documentToStructuredDocument.output.data.Mail;
import at.storchennest.documentToStructuredDocument.output.data.Receipient;

public class DocumentToStructuredDocumentConverter {

	public static void main(String[] args) {
		System.out.println("Hello World");
		MongoDBConnector mdbConnector = new MongoDBConnector("mongodb://104.46.32.71:27017","test");
		mdbConnector.connectToMongoDB();

		JSON2MailConverter converter = new JSON2MailConverter("From","To","Cc","Bcc","Subject","Body");
		
		List<Mail> mails = converter.convertDocumentListToMail(mdbConnector.readOutDocumentsFromCollection("messages"));
		mdbConnector.closeDatabase();
		
		System.out.println("Finished reading mail - "+mails.size() +" mails found.");
		
//		List<Mail> mails = new ArrayList<>();
//
//		Mail m1 = new Mail("TEST","HelloWorld");
//		m1.addReceipent(new Receipient("to",new EMailAccount("pani@zuehlke.com")));
//		m1.addReceipent(new Receipient("to",new EMailAccount("ewpe@zuehlke.com")));
//		m1.addReceipent(new Receipient("cc",new EMailAccount("thfl@zuehlke.com")));
//		m1.addReceipent(new Receipient("bcc",new EMailAccount("pani@zuehlke.com")));
//		m1.setSender(new EMailAccount("luhe@zuehlke.com"));
//		
//		Mail m2 = new Mail("TEST2","HelloWorld");
//		m2.addReceipent(new Receipient("to",new EMailAccount("thfl@zuehlke.com")));
//		m2.addReceipent(new Receipient("bcc",new EMailAccount("pani@zuehlke.com")));
//		m2.setSender(new EMailAccount("luhe@zuehlke.com"));
//
//		
//		mails.add(m1);
//		mails.add(m2);
		
		MongoDBConnector mdbConnectorWriteDB = new MongoDBConnector("mongodb://104.46.32.71:27017","sNoSql");
		mdbConnectorWriteDB.connectToMongoDB();
		for(Mail mail: mails){
			EMailAccount sender = mail.getSender();
			ObjectId senderId = mdbConnectorWriteDB.insertOrUpdateDocument(sender.createDocument(),"EMailAccount",EMailAccount.P_MAIL_ADDRESS_FIELD);
			sender.setObjectID(senderId);
			
			List<Receipient> receipients = mail.getReceipients();
			
			for(Receipient receipient : receipients){
				EMailAccount receipientAccount = receipient.getEmailAccount();
				ObjectId receipientAccountId = mdbConnectorWriteDB.insertOrUpdateDocument(receipientAccount.createDocument(),"EMailAccount",EMailAccount.P_MAIL_ADDRESS_FIELD);
				receipientAccount.setObjectID(receipientAccountId);
			}
			mdbConnectorWriteDB.insert(mail.createDocument(), "mail");
		}
		mdbConnectorWriteDB.closeDatabase();
		
		
	}

}
