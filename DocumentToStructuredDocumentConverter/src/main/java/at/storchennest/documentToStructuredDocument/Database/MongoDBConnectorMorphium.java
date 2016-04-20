package at.storchennest.documentToStructuredDocument.Database;

import java.net.UnknownHostException;
import java.util.List;

import at.storchennest.test.data.Person;
import de.caluga.morphium.Morphium;
import de.caluga.morphium.MorphiumConfig;
import de.caluga.morphium.async.AsyncOperationCallback;
import de.caluga.morphium.async.AsyncOperationType;
import de.caluga.morphium.query.Query;


public class MongoDBConnectorMorphium{
	
	Morphium m;
		
	public MongoDBConnectorMorphium(){
		m=new Morphium("104.46.32.71:27017","sNoSql");
		
	}
	
	public void close(){
		m.close();
	}
	
	public void writeToCollection(String collection, Person data){
		m.store(data, new AsyncOperationCallback<Person>(){

			@Override
			public void onOperationSucceeded(AsyncOperationType type,
					Query<Person> q, long duration, List<Person> result,
					Person entity, Object... param) {
				System.out.println("Insertion succeeded");
				
			}

			@Override
			public void onOperationError(AsyncOperationType type,
					Query<Person> q, long duration, String error, Throwable t,
					Person entity, Object... param) {
				System.out.println("Insertion failed "+t.toString());

				
			}
			
		});
//		   Query<Person> q=m.createQueryFor(Person.class).f(Person.Fields.aField).eq("a id");
	}



}
