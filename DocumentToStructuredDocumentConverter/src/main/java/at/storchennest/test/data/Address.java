package at.storchennest.test.data;

import de.caluga.morphium.annotations.Entity;
import de.caluga.morphium.annotations.Id;
import de.caluga.morphium.annotations.caching.Cache;

@Entity(translateCamelCase = true)
@Cache
public class Address {
	@Id
	int id;
	private String city;
	private int plz;
	private String steet;
	public Address(String city, int plz, String steet) {
		super();
		this.city = city;
		this.plz = plz;
		this.steet = steet;
	}

}
