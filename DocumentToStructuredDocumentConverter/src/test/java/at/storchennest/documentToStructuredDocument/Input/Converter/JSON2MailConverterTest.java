package at.storchennest.documentToStructuredDocument.Input.Converter;

import static org.junit.Assert.*;

import org.junit.Test;

public class JSON2MailConverterTest {

	@Test
	public void testConvertDocumentListToMail() {
		JSON2MailConverter uut = new JSON2MailConverter("from","to","cc","bcc","subject","body");
		

	}

}
