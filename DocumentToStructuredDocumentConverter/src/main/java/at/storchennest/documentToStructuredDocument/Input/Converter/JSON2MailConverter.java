package at.storchennest.documentToStructuredDocument.Input.Converter;

import java.util.ArrayList;
import java.util.List;
import java.util.Map;

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



	public Mail convertDocumentToMail(Document document){
			Mail mail = new Mail();
			if(document.getString(body)!=null){
				mail.setContent(document.getString(body));
			}
			if(document.getObjectId("_id")!=null){
				mail.setOriginalID(document.getObjectId("_id").toString());
			}
			Document headers = (Document) document.get("headers");
			if(headers!=null){
				for(Map.Entry<String, Object> entry :headers.entrySet()){
					if(entry.getKey().equals(subject)) mail.setSubject(headers.getString(subject));
					else if (entry.getKey().equals(from)) mail.setSender(new EMailAccount(headers.getString(from)));
					else if (entry.getKey().equals(to)) mail.addReceipents(createReceipients(headers.getString(to),"to"));
					else if (entry.getKey().equals(cc)) mail.addReceipents(createReceipients(headers.getString(to),"cc"));
					else if (entry.getKey().equals(bcc)) mail.addReceipents(createReceipients(headers.getString(to),"bcc"));
					
					else {
						mail.addHeaderField(entry.getKey(), entry.getValue().toString());
					}
				}

			}

		return mail;
		
	}	
	private List<Receipient> createReceipients(String listOfEMailAddressess, String type){
		List<Receipient> mailAccounts = new ArrayList<>();
		String [] emailArray = listOfEMailAddressess.split(",");
		
		for(String emailString: emailArray){
			String email = emailString.replaceAll("[ \\n\\r\\t]","");
			mailAccounts.add(new Receipient (type,new EMailAccount(email)));
		}
		
		return mailAccounts;
		
				
	}

}
