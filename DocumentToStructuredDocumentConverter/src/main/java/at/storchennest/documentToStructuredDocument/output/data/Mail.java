package at.storchennest.documentToStructuredDocument.output.data;

import java.util.ArrayList;
import java.util.List;

import org.bson.Document;
import org.bson.types.ObjectId;

public class Mail extends ObjectWithUniqueID{
	private static final String P_SENDER_ID_FIELD = "sender";
	private static final String P_RECEIPIENTS_ID_FIELD = "receipients";
	private static final String P_SUBJECT_FIELD = "subject";
	private static final String P_CONTENT_FIELD = "content";
	
	private EMailAccount sender;
	private List<Receipient> receipents;
	private String subject;
	private String content;
	public Mail(String subject, String content) {
		super();
		this.subject = subject;
		this.content = content;
		this.receipents = new ArrayList<Receipient>();
	}
	
	public void addReceipent (Receipient recipent){
		receipents.add(recipent);
	}
	
	public Document createDocument(){
		Document document = new Document();
		if(subject!=null) document.append(P_SUBJECT_FIELD, subject);
		if(content!=null) document.append(P_CONTENT_FIELD, content);
		document.append(P_SENDER_ID_FIELD, sender.getObjectID());
		List<Document> receipientIDList = new ArrayList<>();
		for(Receipient receipient : receipents){
			receipientIDList.add(receipient.createDocument());
		}
		document.append(P_RECEIPIENTS_ID_FIELD,receipientIDList);
		return document;
		
	}

	public EMailAccount getSender() {
		return sender;
	}

	public void setSender(EMailAccount sender) {
		this.sender = sender;
	}
	
	public List<Receipient> getReceipients(){
		return receipents;
	}
	

}
