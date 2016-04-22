CREATE FULLTEXT CATALOG default_full_text_catalog AS DEFAULT

DECLARE @pk_index_name NVARCHAR(MAX) = (SELECT name FROM sys.objects WHERE type = 'PK' AND  parent_object_id = OBJECT_ID ('Mail'))
DECLARE @query NVARCHAR(MAX) = N'CREATE FULLTEXT INDEX ON Mail (Subject, Body) KEY INDEX ' + @pk_index_name + '  WITH STOPLIST = OFF'
EXECUTE sp_executesql @query
