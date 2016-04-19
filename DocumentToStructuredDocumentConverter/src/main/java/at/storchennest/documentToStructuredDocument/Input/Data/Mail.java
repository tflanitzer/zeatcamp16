package at.storchennest.documentToStructuredDocument.Input.Data;

import java.util.Date;
import java.util.HashMap;
import java.util.Map;

public class Mail {
	private String from;
	private String to;
	private String subject;
	private Date timestamp;
	private String content;
	
	private Map<String,String> headerFields;
	
	public Mail(){
		headerFields = new HashMap<>();
	}
	public Mail(String from, String to, String subject, Date timestamp, String content) {
		super();
		this.from = from;
		this.to = to;
		this.subject = subject;
		this.timestamp = timestamp;
		this.content= content;
	}
	public String getFrom() {
		return from;
	}
	public String getTo() {
		return to;
	}
	public String getSubject() {
		return subject;
	}
	public Date getTimestamp() {
		return timestamp;
	}
	public String getContent() {
		return content;
	}
	
	public void addHeaderField(String key, String value){
		headerFields.put(key, value);
	}


}
