import cProfile
import json
import datetime


import redis
r0 = redis.Redis(db=0)
queue_string = 'json queue'



from sqlalchemy.ext.declarative import declarative_base
from sqlalchemy import Column, BigInteger, Integer, String, ForeignKey

from sqlalchemy.orm.exc import NoResultFound

from sqlalchemy.orm import sessionmaker
from sqlalchemy import create_engine

import pymssql 


Base = declarative_base()
session = 0

class EmailAccount(Base):
    __tablename__ = "EmailAccount"
    Id = Column(Integer, primary_key=True)
    EmailAddress = Column(String(250))


class Mail(Base):
    __tablename__ = "Mail"
    Id = Column( Integer, primary_key = True)
    Subject =Column(String)
    Date =Column(BigInteger)
    Body =Column(String)
    SubFolder =Column(String)


class Word(Base):
    __tablename__ = "Word"
    Id = Column( Integer, primary_key = True)
    Word = Column(String) 


class Sender(Base):
    __tablename__="Sender"
    EmailAccountId = Column(ForeignKey('EmailAccount.Id')) # FOREIGN KEY REFERENCES EmailAccount(Id)
    MailId = Column(ForeignKey('Mail.Id'), primary_key=True)  # FOREIGN KEY REFERENCES Mail(Id)
    Name = Column(String)


class Recipient(Base):
    __tablename__ = "Recipient"
    Id = Column( Integer, primary_key = True)
    EmailAccountId = Column(ForeignKey('EmailAccount.Id')) # FOREIGN KEY REFERENCES EmailAccount(Id)
    MailId = Column(ForeignKey('Mail.Id'))  # FOREIGN KEY REFERENCES Mail(Id)
    Name = Column(Integer) 
    Type = Column(String(3)) 


class Attachment(Base):

    __tablename__="Attachment"
    Id = Column( Integer, primary_key = True)
    MailId =Column(ForeignKey('Mail.Id')) #  FOREIGN KEY REFERENCES Mail(Id)
    Name =Column(String) 


class Header(Base):
    __tablename__ ="Header"
    Id = Column( Integer, primary_key = True)
    MailId =Column(ForeignKey('Mail.Id')) #  FOREIGN KEY REFERENCES Mail(Id)
    Key =Column(String(100)) 
    Value =Column(String)


class WordOccurrence(Base):
    __tablename__="WordOccurence"
    Id =Column( Integer, primary_key = True)
    MailId =Column(ForeignKey('Mail.Id')) #  FOREIGN KEY REFERENCES Mail(Id)
    WordId =Column(ForeignKey('Word.Id') )#  FOREIGN KEY REFERENCES Word(Id)
    Position =Column(Integer)


email_address_cache = dict()
def getOrAddEmailAddress(addr):


    query = session.query(EmailAccount)
    res = query.filter(EmailAccount.EmailAddress == addr)

    cached = email_address_cache.get(addr)
    if cached != None:
        print 'cache hit'
        return cached
    else:
        print 'cahce miss ' + addr

    try:
        mail_account = res.one()
        email_address_cache[addr] = mail_account
        return mail_account

    except NoResultFound as e:
        mail_account = EmailAccount(EmailAddress=addr)
        session.add(mail_account)
        session.flush()
        email_address_cache[addr] = mail_account
        return mail_account


def getSender(header):
    header_fields = header.keys()
    
    sender_from = ''
    sender_xfrom = ''
    
    if "From" in header_fields:
        sender_from = header['From']
        pass
    
    if "X-From" in header_fields:
        sender_xfrom = header['X-From']
        pass
    
    return (sender_from, sender_xfrom)


def getRecipients(header):
    recipients = set()
    header_fields = header.keys()
    if "To" in header_fields:
        rec = header['To'].split(',')
        rec = map( lambda x : x.strip(), rec)
        for e in rec:
            recipients.add( (e, 'To', '') )
    
    if "Bcc" in header_fields:
      rec = header['Cc'].split(',')
      rec = map( lambda x : x.strip(), rec)
      for e in rec:
        recipients.add( (e, 'Bcc', '') )
    
    if "Cc" in header_fields:
      rec = header['Cc'].split(',')
      rec = map( lambda x : x.strip(), rec)
      for e in rec:
        recipients.add( (e, 'Cc', '') )
    
    return recipients

    if "X-bc" in header_fields:
      rec = header['X-bc'].split(',')
      rec = map( lambda x : x.strip(), rec)
      for e in rec:
        recipients.add( (e, 'bc', '') )
    
    if "X-bcc" in header_fields:
      rec = header['X-bcc'].split(',')
      rec = map( lambda x : x.strip(), rec)
      for e in rec:
        recipients.add( (e, 'bcc', ''))
    


def addMailToDB(json_dict):
    header = dict()
    mail = 0
    try: 
        header =  json_dict['headers']
    except:
        print "Error no header"
    try:
        unix_date = datetime.datetime.strptime(header["Date"][:-12], "%a, %d %b %Y %H:%M:%S")
        unix_date = (unix_date - datetime.datetime(1970,1,1)).total_seconds()
        mail = Mail(Subject=  header["Subject"], Date=unix_date, Body = json_dict['body'] )
        session.add(mail)
        session.flush()
    except ValueError  as e:
        print "Error while parsing mail"
        raise e

    try:
        recipients = getRecipients(header)
    except Exception as e:
        print "error recip\n" + str(e) + "\n" + str(json_dict)
        raise e

    sender_from, sender_xfrom =  getSender(header)
    sender_addr = ''
    sender_name = ''
    if sender_from != "" :
        sender_addr =  getOrAddEmailAddress(sender_from)
        sender_name = sender_xfrom
    elif sender_xfrom != "" :
        sender_addr =  getOrAddEmailAddress(sender_xfrom)
    else:
        raise Exception("no sender at all")

    db_entries = set()

    sender_entry = Sender( EmailAccountId = sender_addr.Id, MailId = mail.Id, Name=sender_name)
    db_entries.add(sender_entry)
    #session.add(sender_entry)
    #session.flush()



    for recipient in recipients:
        recipient_addr = getOrAddEmailAddress(recipient[0] )
        recipient_entry = Recipient(EmailAccountId = recipient_addr.Id, MailId = mail.Id, Name=recipient[2], Type = recipient[1])
        db_entries.add( recipient_entry)
        

    for key, value in header.iteritems():
        header_entry = Header(MailId = mail.Id, Key = key, Value=value)
        db_entries.add(header_entry)
        #session.add(header_entry)
        #session.flush()
    #session.flush()

    for ent in db_entries:
        session.add(ent)

    session.flush()


    # TODO:
    # - Reciepient Name wird nicht ausgelesen
    # - Subfolder wird auch noch nicht geschrieben

    




class ObjectId:
    def __init__(self, a):
        pass


#========================================================================
#========================================================================
echo_querry=False
engine= create_engine('mssql+pymssql://zeatcamp16@zeatcamp16:Storchennest16@zeatcamp16.database.windows.net:01433/enron_pani', echo=echo_querry)
Session = sessionmaker(bind=engine)
session = Session()
def main():


    for i in xrange(1000) :
        if i%10 == 0:
            print i
        json_entry = r0.lrange(queue_string,i,i)[0]
        json_object = eval(json_entry)
        if type(json_object ) != type( dict()) :
                print "arg"
                print json_object
                return
        addMailToDB(json_object)
        

    session.commit()

    return



if __name__ == '__main__':
    cProfile.run('main()', 'runstats')
    
