package at.storchennest.documentToStructuredDocument.Input.Converter;

import java.util.ArrayList;
import java.util.List;

import org.bson.Document;

import com.mongodb.client.FindIterable;

import at.storchennest.documentToStructuredDocument.output.data.EMailAccount;
import at.storchennest.documentToStructuredDocument.output.data.Mail;
import at.storchennest.documentToStructuredDocument.output.data.Receipient;

public class JSON2MailConverter {
	
	private String from;
	private String to;
	private String cc;
	private String bcc;
	private String subject;
	private String body;
	
	
	
	public JSON2MailConverter(String from, String to,String cc,String bcc, String subject, String body) {
		super();
		this.from = from;
		this.to = to;
		this.cc = cc;
		this.bcc = bcc;

		this.subject = subject;
		this.body = body;
	}



	public List<Mail> convertDocumentListToMail(FindIterable<Document> documentList){
		List<Mail> mails = new ArrayList<>();
		
		for(Document document: documentList){
			Document headers = (Document) document.get("headers");
			Mail mail = new Mail(document.getString(subject),document.getString(body));
			EMailAccount sender = new EMailAccount(headers.getString(from));
			mail.setSender(sender);
			
			Receipient toReceipent = new Receipient ("to");
			toReceipent.setEmailAccount(new EMailAccount(headers.getString(to)));
			
			Receipient ccReceipent  = new Receipient ("cc");
			ccReceipent.setEmailAccount(new EMailAccount(headers.getString(cc)));
			
			Receipient bccReceipent  = new Receipient ("bcc");
			bccReceipent.setEmailAccount(new EMailAccount(headers.getString(bcc)));

			mail.addReceipent(toReceipent);
			mail.addReceipent(ccReceipent);
			mail.addReceipent(bccReceipent);
			
			mails.add(mail);

		}

		
		return mails;
		
	}

}
