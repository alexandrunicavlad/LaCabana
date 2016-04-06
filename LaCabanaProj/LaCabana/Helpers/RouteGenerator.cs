using System;
using Android.Gms.Maps.Model;
using Android.Util;
using Org.Apache.Http.Impl.Client;
using Org.Apache.Http.Protocol;
using System.Net;
using System.IO;
using Javax.Xml.Parsers;
using Java.Lang;
using System.Collections.Generic;
using System.Linq;

namespace LaCabana
{
	public class RouteGenerator
	{
		public static string Mode_driving = "driving";
		public static string mode_walking = "walking";

		public RouteGenerator ()
		{
		}

		public Org.W3c.Dom.IDocument GetDocument (LatLng start, LatLng end, string mode)
		{
			string url = "http://maps.googleapis.com/maps/api/directions/xml?"
			             + "origin=" + start.Latitude.ToString ().Replace (',', '.') + "," + start.Longitude.ToString ().Replace (',', '.')
			             + "&destination=" + end.Latitude.ToString ().Replace (',', '.') + "," + end.Longitude.ToString ().Replace (',', '.')
			             + "&sensor=false&units=metric&mode=driving";
			Log.Debug ("url", url);
			try {
				var httpClient = new DefaultHttpClient ();
				var localContext = new BasicHttpContext ();
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create (url);
				HttpWebResponse response = (HttpWebResponse)request.GetResponse ();

				Stream stream = response.GetResponseStream ();

				//var response = httpClient.Execute(httpPost);
				//var input = response.Entity.Content;
				DocumentBuilder builder = DocumentBuilderFactory.NewInstance ()
					.NewDocumentBuilder ();
				Org.W3c.Dom.IDocument doc = builder.Parse (stream);
				return doc;
			} catch (Java.Lang.Exception e) {
			}

			return null;
		}

		public string GetDurationText (Org.W3c.Dom.IDocument doc)
		{
			try {
				Org.W3c.Dom.INodeList nl1 = doc.GetElementsByTagName ("duration");
				Org.W3c.Dom.INode node1 = nl1.Item (0);
				Org.W3c.Dom.INodeList nl2 = node1.ChildNodes;
				Org.W3c.Dom.INode node2 = nl2.Item (getNodeIndex (nl2, "text"));
				Log.Info ("DurationText", node2.TextContent);
				return node2.TextContent;
			} catch (System.Exception e) {
				return "0";
			}
		}

		public int GetDurationValue (Org.W3c.Dom.IDocument doc)
		{
			try {
				Org.W3c.Dom.INodeList nl1 = doc.GetElementsByTagName ("duration");
				Org.W3c.Dom.INode node1 = nl1.Item (0);
				Org.W3c.Dom.INodeList nl2 = node1.ChildNodes;
				Org.W3c.Dom.INode node2 = nl2.Item (getNodeIndex (nl2, "value"));
				Log.Info ("DurationValue", node2.TextContent);
				return Integer.ParseInt (node2.TextContent);
			} catch (System.Exception e) {
				return -1;
			}
		}

		public string GetDistanceText (Org.W3c.Dom.IDocument doc)
		{

			try {
				Org.W3c.Dom.INodeList nl1 = doc.GetElementsByTagName ("distance");
				Org.W3c.Dom.INode node1 = nl1.Item (nl1.Length - 1);
				Org.W3c.Dom.INodeList nl2 = node1.ChildNodes;
				Org.W3c.Dom.INode node2 = nl2.Item (getNodeIndex (nl2, "value"));
				Log.Debug ("DistanceText", node2.TextContent);
				return node2.TextContent;
			} catch (System.Exception e) {
				return "-1";
			}
		}

		public float GetDistanceValue (Org.W3c.Dom.IDocument doc)
		{
			try {
				Org.W3c.Dom.INodeList nl1 = doc.GetElementsByTagName ("distance");
				Org.W3c.Dom.INode node1 = nl1.Item (nl1.Length - 1);
				Org.W3c.Dom.INodeList nl2 = node1.ChildNodes;
				Org.W3c.Dom.INode node2 = nl2.Item (getNodeIndex (nl2, "value"));
				Log.Info ("DistanceValue", node2.TextContent);
				return Integer.ParseInt (node2.TextContent);
			} catch (System.Exception e) {
				return -1;
			}

		}

		public string GetStartAddress (Org.W3c.Dom.IDocument doc)
		{
			try {
				Org.W3c.Dom.INodeList nl1 = doc.GetElementsByTagName ("start_address");
				Org.W3c.Dom.INode node1 = nl1.Item (0);
				Log.Info ("StartAddress", node1.TextContent);
				return node1.TextContent;
			} catch (System.Exception e) {
				return "-1";
			}

		}

		public string GetEndAddress (Org.W3c.Dom.IDocument doc)
		{
			try {
				Org.W3c.Dom.INodeList nl1 = doc.GetElementsByTagName ("end_address");
				Org.W3c.Dom.INode node1 = nl1.Item (0);
				Log.Info ("StartAddress", node1.TextContent);
				return node1.TextContent;
			} catch (System.Exception e) {
				return "-1";        
			}
		}

		public string GetCopyRights (Org.W3c.Dom.IDocument doc)
		{
			try {
				Org.W3c.Dom.INodeList nl1 = doc.GetElementsByTagName ("copyrights");
				Org.W3c.Dom.INode node1 = nl1.Item (0);
				Log.Info ("CopyRights", node1.TextContent);
				return node1.TextContent;
			} catch (System.Exception e) {
				return "-1";
			}

		}

		public List<LatLng> GetDirection (Org.W3c.Dom.IDocument doc)
		{
			Org.W3c.Dom.INodeList nl1, nl2, nl3;
			List<LatLng> listGeopoints = new List<LatLng> ();
			nl1 = doc.GetElementsByTagName ("step");
			if (nl1.Length > 0) {
				for (int i = 0; i < nl1.Length; i++) {
					Org.W3c.Dom.INode node1 = nl1.Item (i);
					nl2 = node1.ChildNodes;

					Org.W3c.Dom.INode locationNode = nl2.Item (getNodeIndex (nl2, "start_location"));
					nl3 = locationNode.ChildNodes;
					Org.W3c.Dom.INode latNode = nl3.Item (getNodeIndex (nl3, "lat"));
					double lat = Java.Lang.Double.ParseDouble (latNode.TextContent);
					Org.W3c.Dom.INode lngNode = nl3.Item (getNodeIndex (nl3, "lng"));
					double lng = Java.Lang.Double.ParseDouble (lngNode.TextContent);
					listGeopoints.Add (new LatLng (lat, lng));

					locationNode = nl2.Item (getNodeIndex (nl2, "polyline"));
					nl3 = locationNode.ChildNodes;
					latNode = nl3.Item (getNodeIndex (nl3, "points"));
					List<LatLng> arr = DecodePoly (latNode.TextContent);
					for (int j = 0; j < arr.Count; j++) {
						listGeopoints.Add (new LatLng (arr.ElementAt (j).Latitude, arr.ElementAt (j).Longitude));
					}

					locationNode = nl2.Item (getNodeIndex (nl2, "end_location"));
					nl3 = locationNode.ChildNodes;
					latNode = nl3.Item (getNodeIndex (nl3, "lat"));
					lat = Java.Lang.Double.ParseDouble (latNode.TextContent);
					lngNode = nl3.Item (getNodeIndex (nl3, "lng"));
					lng = Java.Lang.Double.ParseDouble (lngNode.TextContent);
					listGeopoints.Add (new LatLng (lat, lng));
				}
			}

			return listGeopoints;
		}

		private int getNodeIndex (Org.W3c.Dom.INodeList nl, string nodename)
		{
			for (int i = 0; i < nl.Length; i++) {
				if (nl.Item (i).NodeName.Equals (nodename))
					return i;
			}
			return -1;
		}

		private List<LatLng> DecodePoly (string encoded)
		{
			List<LatLng> poly = new List<LatLng> ();
			int index = 0, len = encoded.Length;
			int lat = 0, lng = 0;
			while (index < len) {
				int b, shift = 0, result = 0;
				do {
					b = encoded.ElementAt (index++) - 63;
					result |= (b & 0x1f) << shift;
					shift += 5;
				} while (b >= 0x20);
				int dlat = ((result & 1) != 0 ? ~(result >> 1) : (result >> 1));
				lat += dlat;
				shift = 0;
				result = 0;
				do {
					b = encoded.ElementAt (index++) - 63;
					result |= (b & 0x1f) << shift;
					shift += 5;
				} while (b >= 0x20);
				int dlng = ((result & 1) != 0 ? ~(result >> 1) : (result >> 1));
				lng += dlng;

				LatLng position = new LatLng ((double)lat / 1E5, (double)lng / 1E5);
				poly.Add (position);
			}
			return poly;
		}


	}
}

