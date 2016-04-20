package at.storchennest.documentToStructuredDocument.output.data;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import org.bson.Document;
import org.bson.types.ObjectId;

public class Mail extends ObjectWithUniqueID{
	private static final String P_SENDER_ID_FIELD = "sender";
	private static final String P_RECEIPIENTS_ID_FIELD = "receipients";
	private static final String P_HEADERS_FIELD = "headers";

	private static final String P_SUBJECT_FIELD = "subject";
	private static final String P_CONTENT_FIELD = "content";
	private static final String P_ORIGID_FIELD = "OriginalId";

	
	private EMailAccount sender;
	private List<Receipient> receipents;
	private String subject;
	private String content;
	private String originalID;
	private HashMap<String,String> headerFields;
	public Mail() {
		super();
		this.receipents = new ArrayList<Receipient>();
		headerFields = new HashMap<>();
	}
	
	public void addReceipent (Receipient recipent){
		receipents.add(recipent);
	}
	public void addReceipents (List<Receipient> recipentList){
		receipents.addAll(recipentList);
	}


	
	public String getContent() {
		return content;
	}

	public Document createDocument(){
		Document document = new Document();
		if(subject!=null) document.append(P_SUBJECT_FIELD, subject);
		if(content!=null) document.append(P_CONTENT_FIELD, content);
		if(sender!=null) document.append(P_SENDER_ID_FIELD, sender.getObjectID());
		if(originalID!=null) document.append(P_ORIGID_FIELD, originalID);
		

		List<Document> receipientIDList = new ArrayList<>();
		for(Receipient receipient : receipents){
			receipientIDList.add(receipient.createDocument());
		}
		if(receipientIDList.size()>0) document.append(P_RECEIPIENTS_ID_FIELD,receipientIDList);
		
		List<Document> headerFieldsDocuments = new ArrayList<>();
		headerFields.entrySet().forEach((mapEntry) -> {
			headerFieldsDocuments.add(new Document(mapEntry.getKey(),mapEntry.getValue()));
		});
		if(headerFieldsDocuments.size()>0) document.append(P_HEADERS_FIELD,headerFieldsDocuments);


		return document;
		
	}
	
	public Document createFatDocument(){
		Document document = new Document();
		if(subject!=null) document.append(P_SUBJECT_FIELD, subject);
		if(content!=null) document.append(P_CONTENT_FIELD, content);
		if(sender!=null) document.append(P_SENDER_ID_FIELD, sender.getMailAddress());
		if(originalID!=null) document.append(P_ORIGID_FIELD, originalID);

		List<Document> receipientIDList = new ArrayList<>();
		for(Receipient receipient : receipents){
			receipientIDList.add(receipient.createFatDocument());
		}
		if(receipientIDList.size()>0) document.append(P_RECEIPIENTS_ID_FIELD,receipientIDList);
		
		List<Document> headerFieldsDocuments = new ArrayList<>();
		headerFields.entrySet().forEach((mapEntry) -> headerFieldsDocuments.add(new Document(mapEntry.getKey(),mapEntry.getValue())));
		if(headerFieldsDocuments.size()>0) document.append(P_HEADERS_FIELD,headerFieldsDocuments);

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

	public void setSubject(String subject) {
		this.subject = subject;
	}

	public void setContent(String content) {
		this.content = content;
	}

	public void setOriginalID(String originalID) {
		this.originalID = originalID;
	}
	
	public void addHeaderField(String key, String value){
		headerFields.put(key, value);
	}
}
