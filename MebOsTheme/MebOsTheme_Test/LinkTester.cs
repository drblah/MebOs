using System;
using NUnit.Framework;
using MebOsTheme;

namespace MebOsTheme_Test
{
	[TestFixture]
	public class LinkTest
	{
		
		[SetUp]
		public void Setup ()
		{
		}
		
		[Test]
		public void testLinkWithJpg ()
		{
			LinkExtractor testLE = new LinkExtractor ();
			Assert.AreEqual ("https://test.jpg",testLE.linkInString("hej med dig https://test.jpg"));
		}

		[Test]
		public void testLinkWithoutJpg ()
		{
			LinkExtractor testLE = new LinkExtractor ();
			Assert.AreEqual ("Found no jpgs",testLE.linkInString("hej med dig https://test"));
		}

		[Test]
		public void testLinkNotJpg ()
		{
			LinkExtractor testLE = new LinkExtractor ();
			Assert.AreEqual ("Found no jpgs",testLE.linkInString("hej med dig https://test.png"));
		}

		[Test]
		public void testLinkNoHTTPS ()
		{
			LinkExtractor testLE = new LinkExtractor ();
			Assert.AreEqual ("No links",testLE.linkInString("hej med dig"));
		}

		[Test]
		public void testLinkMultipleHTTPS ()
		{
			LinkExtractor testLE = new LinkExtractor ();
			Assert.AreEqual ("https://test.jpg",testLE.linkInString("hej med dig https://test.jpg https://test2.jpg"));
		}

		[Test]
		public void testLinkShortHTTPS ()
		{
			LinkExtractor testLE = new LinkExtractor ();
			Assert.AreEqual ("https://test.jpg/",testLE.linkInString("hej med dig https://goo.gl/X2KftW"));
		}

		[Test]
		public void testLinkShortHTTPSNoJpg ()
		{
			LinkExtractor testLE = new LinkExtractor ();
			Assert.AreEqual ("Found no jpgs",testLE.linkInString("hej med dig https://goo.gl/bERbbS"));
		}
	}
}