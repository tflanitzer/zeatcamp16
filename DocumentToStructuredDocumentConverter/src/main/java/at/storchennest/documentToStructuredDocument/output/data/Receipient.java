package at.storchennest.documentToStructuredDocument.output.data;

import org.bson.Document;

public class Receipient{
	private static final String P_MAIL_ACCOUNT_FIELD = "MailAccountID";
	private static final String P_TYPE_FIELD = "type";


	private EMailAccount emailAccount;
	private String type;
	
	public Receipient(String type) {
		super();
		this.type = type;
	}
	
	public Receipient(String type, EMailAccount account) {
		this(type);
		this.emailAccount = account;
	}
	
	public Document createDocument(){
		Document document = new Document();
		document.append(P_MAIL_ACCOUNT_FIELD, emailAccount.getObjectID());
		document.append(P_TYPE_FIELD, type);
		return document;
	}
	public Document createFatDocument(){
		Document document = new Document();
		document.append(P_MAIL_ACCOUNT_FIELD, emailAccount.getMailAddress());
		document.append(P_TYPE_FIELD, type);
		return document;
	}
	

	public void setEmailAccount(EMailAccount emailAccount) {
		this.emailAccount = emailAccount;
	}

	public EMailAccount getEmailAccount() {
		return emailAccount;
	}
	
	

}
