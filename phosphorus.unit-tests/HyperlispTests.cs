
/*
 * phosphorus five, copyright 2014 - Mother Earth, Jannah, Gaia
 * phosphorus five is licensed as mit, see the enclosed LICENSE file for details
 */

using System;
using NUnit.Framework;
using phosphorus.core;

namespace phosphorus.unittests
{
    [TestFixture]
    public class HyperlispTests : TestBase
    {
        public HyperlispTests ()
        {
            Loader.Instance.LoadAssembly ("phosphorus.hyperlisp");
            Loader.Instance.LoadAssembly ("phosphorus.lambda");
            _context = Loader.Instance.CreateApplicationContext ();
        }

        [Test]
        public void ParseSimpleHyperlisp ()
        {
            Node tmp = new Node ();
            tmp.Value = @"x:y";
            _context.Raise ("pf.hyperlisp.hyperlisp2lambda", tmp);
            Assert.AreEqual ("x", tmp [0].Name, "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual ("y", tmp [0].Value, "wrong value of node after parsing of hyperlisp");
        }

        [Test]
        public void ParseSimpleHyperlispAdditionalWhiteSpace ()
        {
            Node tmp = new Node ();
            tmp.Value = @"

x:y

";
            _context.Raise ("pf.hyperlisp.hyperlisp2lambda", tmp);
            Assert.AreEqual (1, tmp.Count, "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual ("x", tmp [0].Name, "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual ("y", tmp [0].Value, "wrong value of node after parsing of hyperlisp");
        }

        [Test]
        public void ParseEmptyRootName ()
        {
            Node tmp = new Node ();
            tmp.Value = @":y";
            _context.Raise ("pf.hyperlisp.hyperlisp2lambda", tmp);
            Assert.AreEqual (string.Empty, tmp [0].Name, "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual ("y", tmp [0].Value, "wrong value of node after parsing of hyperlisp");
        }

        [Test]
        public void ParseEmptySingleLineCommentToken ()
        {
            Node tmp = new Node ();
            tmp.Value = @"//";
            _context.Raise ("pf.hyperlisp.hyperlisp2lambda", tmp);
            Assert.AreEqual (0, tmp.Count, "wrong value of node after parsing of hyperlisp");
        }

        [Test]
        public void ParseSingleLineCommentToken ()
        {
            Node tmp = new Node ();
            tmp.Value = @"// comment";
            _context.Raise ("pf.hyperlisp.hyperlisp2lambda", tmp);
            Assert.AreEqual (0, tmp.Count, "wrong value of node after parsing of hyperlisp");
        }

        [Test]
        public void ParseEmptyMultiLineCommentToken ()
        {
            Node tmp = new Node ();
            tmp.Value = @"/**/";
            _context.Raise ("pf.hyperlisp.hyperlisp2lambda", tmp);
            Assert.AreEqual (0, tmp.Count, "wrong value of node after parsing of hyperlisp");
        }

        [Test]
        public void ParseMultiLineCommentTokenOnSingleLine ()
        {
            Node tmp = new Node ();
            tmp.Value = @"/*comment*/";
            _context.Raise ("pf.hyperlisp.hyperlisp2lambda", tmp);
            Assert.AreEqual (0, tmp.Count, "wrong value of node after parsing of hyperlisp");
        }

        [Test]
        public void ParseMultiLineCommentTokenOnMultipleLines ()
        {
            Node tmp = new Node ();
            tmp.Value = @"/*
comment

*/";
            _context.Raise ("pf.hyperlisp.hyperlisp2lambda", tmp);
            Assert.AreEqual (0, tmp.Count, "wrong value of node after parsing of hyperlisp");
        }

        [Test]
        public void ParseNodesWithCommentTokens ()
        {
            Node tmp = new Node ();
            tmp.Value = @"// comment
jo:dude
/*comment */
hello";
            _context.Raise ("pf.hyperlisp.hyperlisp2lambda", tmp);
            Assert.AreEqual (2, tmp.Count, "wrong value of node after parsing of hyperlisp");
        }

        [Test]
        public void ParseStringLiteral ()
        {
            Node tmp = new Node ();
            tmp.Value = @"x:""y""";
            _context.Raise ("pf.hyperlisp.hyperlisp2lambda", tmp);
            Assert.AreEqual ("y", tmp [0].Value, "wrong value of node after parsing of hyperlisp");
        }
        
        [Test]
        public void ParseStringLiteralWithEscapeCharacters ()
        {
            Node tmp = new Node ();
            tmp.Value = @"x:""\ny\\\r\n\""""";
            _context.Raise ("pf.hyperlisp.hyperlisp2lambda", tmp);
            Assert.AreEqual ("\r\ny\\\r\n\"", tmp [0].Value, "wrong value of node after parsing of hyperlisp");
        }
        
        [Test]
        public void ParseMultilineStringLiteral ()
        {
            Node tmp = new Node ();
            tmp.Value = @"x:@""y""";
            _context.Raise ("pf.hyperlisp.hyperlisp2lambda", tmp);
            Assert.AreEqual ("y", tmp [0].Value, "wrong value of node after parsing of hyperlisp");
        }
        
        [Test]
        public void ParseMultilineStringLiteralWithEscapeCharacters ()
        {
            Node tmp = new Node ();
            tmp.Value = string.Format (@"x:@""mumbo
jumbo""""howdy\r\n{0}""", "\n");
            _context.Raise ("pf.hyperlisp.hyperlisp2lambda", tmp);
            Assert.AreEqual ("mumbo\r\njumbo\"howdy\\r\\n\r\n", tmp [0].Value, "wrong value of node after parsing of hyperlisp");
        }
        
        [Test]
        public void ParseEmptyStringLiteral ()
        {
            Node tmp = new Node ();
            tmp.Value = @"x:""""";
            _context.Raise ("pf.hyperlisp.hyperlisp2lambda", tmp);
            Assert.AreEqual ("", tmp [0].Value, "wrong value of node after parsing of hyperlisp");
        }
        
        [Test]
        public void ParseEmptyMultilineStringLiteral ()
        {
            Node tmp = new Node ();
            tmp.Value = @"x:@""""";
            _context.Raise ("pf.hyperlisp.hyperlisp2lambda", tmp);
            Assert.AreEqual ("", tmp [0].Value, "wrong value of node after parsing of hyperlisp");
        }
        
        [Test]
        public void ParseMultipleNodes ()
        {
            Node tmp = new Node ();
            tmp.Value = @"x:@""""
x2
  x3
  :x4
  :int:4
  :string:@""
howdy world
""
y:z
  z:0
    q:1
  w:2
    v:12
t:h";
            _context.Raise ("pf.hyperlisp.hyperlisp2lambda", tmp);
            Assert.AreEqual ("x", tmp [0].Name, "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual ("", tmp [0].Value, "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual ("x2", tmp [1].Name, "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual ("x3", tmp [1][0].Name, "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual (null, tmp [1][0].Value, "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual (string.Empty, tmp [1][1].Name, "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual ("x4", tmp [1][1].Value, "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual (string.Empty, tmp [1][2].Name, "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual (4, tmp [1][2].Value, "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual (typeof(int), tmp [1][2].Value.GetType (), "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual (string.Empty, tmp [1][3].Name, "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual ("\r\nhowdy world\r\n", tmp [1][3].Value, "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual (null, tmp [1].Value, "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual ("y", tmp [2].Name, "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual ("z", tmp [2].Value, "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual ("z", tmp [2][0].Name, "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual ("0", tmp [2][0].Value, "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual ("q", tmp [2][0][0].Name, "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual ("1", tmp [2][0][0].Value, "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual ("w", tmp [2][1].Name, "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual ("2", tmp [2][1].Value, "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual ("v", tmp [2][1][0].Name, "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual ("12", tmp [2][1][0].Value, "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual ("t", tmp [3].Name, "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual ("h", tmp [3].Value, "wrong value of node after parsing of hyperlisp");
        }
        
        [Test]
        public void ParseTypes ()
        {
            Node tmp = new Node ();
            tmp.Value = @"_string:string:@""""
_string:string:
_int:int:5
_float:float:10.55
_double:double:10.54
_node:node:@""x:z""
_string:""string:""
_bool:bool:true
_guid:guid:E5A53FC9-A306-4609-89E5-9CC2964DA0AC
_dna:path:0-1
_long:long:-9223372036854775808
_ulong:ulong:18446744073709551615
_uint:uint:4294967295
_short:short:-32768
_decimal:decimal:456.89
_byte:byte:255
_sbyte:sbyte:-128
_char:char:x
_date:date:2012-12-21
_date:date:""2012-12-21T23:59:59""
_date:date:""2012-12-21T23:59:59.987""
_time:time:""15.23:57:53.567""";
            _context.Raise ("pf.hyperlisp.hyperlisp2lambda", tmp);
            Assert.AreEqual ("", tmp [0].Value, "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual ("", tmp [1].Value, "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual (5, tmp [2].Value, "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual (10.55F, tmp [3].Value, "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual (typeof(float), tmp [3].Value.GetType (), "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual (10.54D, tmp [4].Value, "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual (typeof(double), tmp [4].Value.GetType (), "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual (typeof(Node), tmp [5].Value.GetType (), "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual ("x", tmp [5].Get<Node> ().Name, "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual ("z", tmp [5].Get<Node> ().Value, "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual ("string:", tmp [6].Value, "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual (true, tmp [7].Value, "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual (Guid.Parse ("E5A53FC9-A306-4609-89E5-9CC2964DA0AC"), tmp [8].Value, "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual (new Node.DNA ("0-1"), tmp [9].Value, "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual (-9223372036854775808L, tmp [10].Value, "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual (18446744073709551615L, tmp [11].Value, "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual (4294967295, tmp [12].Value, "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual (-32768, tmp [13].Value, "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual (456.89M, tmp [14].Value, "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual (255, tmp [15].Value, "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual (-128, tmp [16].Value, "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual ('x', tmp [17].Value, "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual (new DateTime (2012, 12, 21), tmp [18].Value, "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual (new DateTime (2012, 12, 21, 23, 59, 59), tmp [19].Value, "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual (new DateTime (2012, 12, 21, 23, 59, 59, 987), tmp [20].Value, "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual (new TimeSpan (15, 23, 57, 53, 567), tmp [21].Value, "wrong value of node after parsing of hyperlisp");
        }
        
        [Test]
        public void BinaryTypes ()
        {
            Node tmp = new Node ();
            tmp.Add (new Node ("_blob", new byte [] { 134, 254, 12 }));
            _context.Raise ("pf.hyperlisp.lambda2hyperlisp", tmp);
            Assert.AreEqual ("_blob:blob:hv4M", tmp.Value, "wrong value of node after parsing of hyperlisp");
            tmp = new Node (string.Empty, tmp.Value);
            _context.Raise ("pf.hyperlisp.hyperlisp2lambda", tmp);
            Assert.AreEqual (new byte [] { 134, 254, 12 }, tmp [0].Value, "wrong value of node after parsing of hyperlisp");
        }

        [Test]
        public void ComplexNamesAndNonExistentType ()
        {
            Node tmp = new Node ();
            tmp.Value = @"""_tmp1\nthomas"":howdy
@""_tmp2"":howdy22
  @""_tmp3"":""mumbo-jumbo-type"":@""value""
  @""_tmp4
is cool"":@""mumbo-
jumbo-type"":@""value
   value""";
            _context.Raise ("pf.hyperlisp.hyperlisp2lambda", tmp);
            Assert.AreEqual ("_tmp1\r\nthomas", tmp [0].Name, "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual ("howdy", tmp [0].Value, "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual ("_tmp2", tmp [1].Name, "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual ("howdy22", tmp [1].Value, "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual ("_tmp3", tmp [1][0].Name, "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual ("value", tmp [1][0].Value, "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual ("_tmp4\r\nis cool", tmp [1][1].Name, "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual ("value\r\n   value", tmp [1][1].Value, "wrong value of node after parsing of hyperlisp");
        }

        [Test]
        public void ParseUsingExpression ()
        {
            Node tmp = ExecuteLambda (@"_data:@""_foo
  tmp1
  tmp2:howdy world""
code2lambda:@/-/?value");
            Assert.AreEqual ("_foo", tmp [1][0].Name, "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual (1, tmp [1].Count, "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual (2, tmp [1][0].Count, "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual ("tmp1", tmp [1][0][0].Name, "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual ("tmp2", tmp [1][0][1].Name, "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual ("howdy world", tmp [1][0][1].Value, "wrong value of node after parsing of hyperlisp");
        }

        [Test]
        public void ParseUsingExpressionYieldingMultipleResults ()
        {
            Node tmp = ExecuteLambda (@"_data:@""_foo
  tmp1
  tmp2:howdy world""
_data:@""_foo2
  tmp12
  tmp22:howdy world2""
code2lambda:@/../*/_data/?value");
            Assert.AreEqual (2, tmp [2].Count, "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual (2, tmp [2][0].Count, "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual (2, tmp [2][1].Count, "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual ("_foo", tmp [2][0].Name, "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual ("tmp1", tmp [2][0][0].Name, "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual ("tmp2", tmp [2][0][1].Name, "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual ("howdy world", tmp [2][0][1].Value, "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual ("_foo2", tmp [2][1].Name, "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual ("tmp12", tmp [2][1][0].Name, "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual ("tmp22", tmp [2][1][1].Name, "wrong value of node after parsing of hyperlisp");
            Assert.AreEqual ("howdy world2", tmp [2][1][1].Value, "wrong value of node after parsing of hyperlisp");
        }
        
        [Test]
        public void CreateHyperlispFromNodes ()
        {
            Node tmp = ExecuteLambda (@"pf.hyperlisp.lambda2hyperlisp
  _data
    tmp1
    tmp2:howdy world");
            Assert.AreEqual ("_data\r\n  tmp1\r\n  tmp2:howdy world", tmp [0].Value, "wrong value of node after parsing of hyperlisp");
        }

        [Test]
        public void CreateHyperlispFromNodesUsingExpression ()
        {
            Node tmp = ExecuteLambda (@"
_data
  tmp1
  tmp2:howdy world
pf.hyperlisp.lambda2hyperlisp:@/-/?node");
            Assert.AreEqual ("_data\r\n  tmp1\r\n  tmp2:howdy world", tmp [1].Value, "wrong value of node after parsing of hyperlisp");
        }

        [Test]
        public void CreateHyperlispFromNodesUsingExpressionYieldingMultipleResults ()
        {
            Node tmp = ExecuteLambda (@"_data
  tmp1
  tmp2:howdy world
_data
  tmp12
  tmp22:howdy world2
pf.hyperlisp.lambda2hyperlisp:@/../*/_data/?node");
            Assert.AreEqual ("_data\r\n  tmp1\r\n  tmp2:howdy world\r\n_data\r\n  tmp12\r\n  tmp22:howdy world2", tmp [2].Value, "wrong value of node after parsing of hyperlisp");
        }

        [Test]
        [ExpectedException]
        public void SyntaxError1 ()
        {
            Node tmp = new Node ();
            tmp.Value = @" x:y"; // one space before token
            _context.Raise ("pf.hyperlisp.hyperlisp2lambda", tmp);
        }
        
        [Test]
        [ExpectedException]
        public void SyntaxError2 ()
        {
            Node tmp = new Node ();
            tmp.Value = @"x:y
 z:q"; // only one space when opening children collection
            _context.Raise ("pf.hyperlisp.hyperlisp2lambda", tmp);
        }
        
        [Test]
        [ExpectedException]
        public void SyntaxError3 ()
        {
            Node tmp = new Node ();
            tmp.Value = @"x:y
   z:q"; // three spaces when opening children collection
            _context.Raise ("pf.hyperlisp.hyperlisp2lambda", tmp);
        }

        [Test]
        [ExpectedException]
        public void SyntaxError4 ()
        {
            Node tmp = new Node ();
            tmp.Value = @"x:y
  z:""howdy"; // open string literal
            _context.Raise ("pf.hyperlisp.hyperlisp2lambda", tmp);
        }
        
        [Test]
        [ExpectedException]
        public void SyntaxError5 ()
        {
            Node tmp = new Node ();
            tmp.Value = @"x:y
  z:"""; // empty and open string literal
            _context.Raise ("pf.hyperlisp.hyperlisp2lambda", tmp);
        }
        
        [Test]
        [ExpectedException]
        public void SyntaxError6 ()
        {
            Node tmp = new Node ();
            tmp.Value = @"x:y
  z:@"""; // empty and open multiline string literal
            _context.Raise ("pf.hyperlisp.hyperlisp2lambda", tmp);
        }
        
        [Test]
        [ExpectedException]
        public void SyntaxError7 ()
        {
            Node tmp = new Node ();
            tmp.Value = @"x:y
  z:@""howdy"; // open multiline string literal
            _context.Raise ("pf.hyperlisp.hyperlisp2lambda", tmp);
        }
        
        [Test]
        [ExpectedException]
        public void SyntaxError8 ()
        {
            Node tmp = new Node ();
            tmp.Value = @"x:y
  z:@""howdy

"; // open multiline string literal
            _context.Raise ("pf.hyperlisp.hyperlisp2lambda", tmp);
        }
        
        [Test]
        [ExpectedException]
        public void SyntaxError9 ()
        {
            Node tmp = new Node ();
            tmp.Value = @"z:node:@""howdy:x
 f:g"""; // syntax error in hyperlisp node content, only one space while opening child collection of "howdy" node
            _context.Raise ("pf.hyperlisp.hyperlisp2lambda", tmp);
        }
        
        [Test]
        [ExpectedException]
        public void SyntaxError10 ()
        {
            Node tmp = new Node ();
            tmp.Value = @"z:node:@""howdy:x
f:g"""; // logical error in hyperlisp node content, multiple "root" nodes
            _context.Raise ("pf.hyperlisp.hyperlisp2lambda", tmp);
        }
    }
}
