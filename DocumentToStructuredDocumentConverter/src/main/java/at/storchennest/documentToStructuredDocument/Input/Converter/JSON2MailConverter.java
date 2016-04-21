package at.storchennest.documentToStructuredDocument.Input.Converter;

import java.text.DateFormat;
import java.text.SimpleDateFormat;
import java.time.LocalDate;
import java.time.format.DateTimeFormatter;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;
import java.util.Locale;
import java.util.Map;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

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
	private String _id;
	private String date;
	
	
	public JSON2MailConverter(String from, String to, String cc, String bcc,
			String subject, String body, String _id, String date) {
		super();
		this.from = from;
		this.to = to;
		this.cc = cc;
		this.bcc = bcc;
		this.subject = subject;
		this.body = body;
		this._id = _id;
		this.date = date;
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
					else if (entry.getKey().equals(cc)) mail.addReceipents(createReceipients(headers.getString(cc),"cc"));
					else if (entry.getKey().equals(bcc)) mail.addReceipents(createReceipients(headers.getString(bcc),"bcc"));
					else if (entry.getKey().equals(date)) mail.setDate(convertStrintToLocalDate(headers.getString(date)));
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
	
	private LocalDate convertStrintToLocalDate(String input){
		Pattern pattern = Pattern.compile("[\\d]* [a-zA-Z]* [\\d]* [\\d]*:[\\d]*:[\\d]*");
		Matcher matcher = pattern.matcher(input);
		String dateString="";
		if (matcher.find())
		{
			dateString = matcher.group(0);
			DateTimeFormatter formatter = DateTimeFormatter.ofPattern("d MMM uuuu HH:mm:ss", Locale.ENGLISH);
			LocalDate date = LocalDate.parse(dateString, formatter);
			return date;
		}
		else{
			System.out.println("Unable to find regex "+ input);
			return null;
		}
	}

}
