package at.storchennest.documentToStructuredDocument.output.data;

import org.bson.Document;

public class EMailAccount extends ObjectWithUniqueID{
	public static final String P_MAIL_ADDRESS_FIELD = "MailAddress";
	private String mailAddress;

	public EMailAccount(String mailAddress) {
		super();
		this.mailAddress = mailAddress;
	}
	

	public Document createDocument(){
		Document document = new Document();
		document.append(P_MAIL_ADDRESS_FIELD, mailAddress);
		return document;
	}



	
}
