﻿using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PerformanceTestUtil;

namespace Sql.Queries
{
    public class AllMailsWithWordInSubjectOrBodyWithLike : QueryBase
    {
        public AllMailsWithWordInSubjectOrBodyWithLike(SqlConnection connection) : base(connection) {}

        public override string CommandText => @"SELECT * FROM Mail WHERE Body LIKE '%million%  OR Subject LIKE '%million%'";
    }
}