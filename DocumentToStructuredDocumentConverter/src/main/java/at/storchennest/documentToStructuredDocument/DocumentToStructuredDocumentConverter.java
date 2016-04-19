package at.storchennest.documentToStructuredDocument;

import at.storchennest.documentToStructuredDocument.Database.MongoDBConnector;

public class DocumentToStructuredDocumentConverter {

	public static void main(String[] args) {
		System.out.println("Hello World");
		MongoDBConnector MDBConnector = new MongoDBConnector();
		MDBConnector.connectToMongoDB();
	}

}
