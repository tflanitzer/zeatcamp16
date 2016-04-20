package at.storchennest.documentToStructuredDocument.output.data;

import org.bson.types.ObjectId;

public class ObjectWithUniqueID {
	private ObjectId objectID;

	
	public ObjectId getObjectID() {
		return objectID;
	}

	public void setObjectID(ObjectId objectID) {
		this.objectID = objectID;
	}
	
}
