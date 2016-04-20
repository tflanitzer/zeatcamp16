package at.storchennest.test.data;

import java.util.Calendar;

import de.caluga.morphium.annotations.Entity;
import de.caluga.morphium.annotations.Id;
import de.caluga.morphium.annotations.caching.Cache;

@Entity(translateCamelCase = true)
@Cache
public class Person {
	private String name;
	private Calendar birthDate;
	@Id
	private int svnr;
//	@Reference
//	private Address address;
	
	
	public Person(String name, Calendar birthDate, int svnr) {
		super();
		this.name = name;
		this.birthDate = birthDate;
		this.svnr = svnr;
	}
//	public void setAddress(Address address) {
//		this.address = address;
//	}
	
}