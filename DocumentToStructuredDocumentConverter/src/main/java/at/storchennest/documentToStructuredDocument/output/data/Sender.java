package at.storchennest.documentToStructuredDocument.output.data;

import org.bson.Document;

public class Sender extends ObjectWithUniqueID{
	private static final String P_MAIL_ACCOUNT_FIELD = "MailAccountID";
	private static final String P_MAIL_FIELD = "mail";
	private static final String P_NAME_FIELD = "name";
;


	private String name;
	private EMailAccount emailAccount;
	private Mail mail;

	
	public Document createDocument(){
		Document document = new Document();
		document.append("_id", getObjectID());
		document.append(P_MAIL_ACCOUNT_FIELD, emailAccount.getObjectID());
		document.append(P_MAIL_FIELD, mail.getObjectID());
		document.append(P_NAME_FIELD, name);

		return document;
	}

	public void setEmailAccount(EMailAccount emailAccount) {
		this.emailAccount = emailAccount;
	}

	public void setMail(Mail mail) {
		this.mail = mail;
	}
}
