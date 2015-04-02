/**
 * Author: Andrej Bestuzhev
 * E-mail: andrej.bestuzhev@gmail.com
 * Web: http://poest.net
 * Created: 31.03.2015
 */
     
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace SpriteUtils {

	public class SpriteFitter {
	
		string[] sources;
		string[] destinations;
		bool overwrite,trim;
		string format;
		Dictionary<string,SpriteArray> SpriteArrays;
	
		public SpriteFitter(string[] sources, string[] destinations, string format = "PNG", bool trim = false, bool overwrite = true) {
			this.sources = sources;
			this.destinations = destinations;
			this.format = format;
			this.trim = trim;
			this.overwrite = overwrite;
			
			SpriteArrays = new Dictionary<string,SpriteArray>();
		}
		
		public void Process() {
			Console.Write("Checking files...");
			if(!CheckFiles()) {
				return;
			}
			Console.Write("OK\n");
			Collect();
			Concat();
		}
		
		
		// Collecting frames to arrays
		private void Collect() {
			for(int i = 0; i < sources.Length; i++) {
				SpriteArray sprites = new SpriteArray();
				string[] files = Directory.GetFiles(sources[i],"*."+format.ToLower());
				for(int f = 0; f < files.Length; f++) {
					sprites.Add(files[f]);
				}
				sprites.OutputFile = destinations[i];
				SpriteArrays.Add(sources[i],sprites);
				Console.WriteLine("{0} : {1} files added to sprite {2}", sources[i],sprites.Count, sprites.OutputFile);
			}
		}
		
		private void Concat(bool trimFrames = false) {
			foreach(KeyValuePair<string,SpriteArray> Sprites in SpriteArrays) {
				int TotalWidth = 0;
				int TotalHeight = 0;
				Bitmap[] Frames = new Bitmap[Sprites.Value.Count];
				for(int i = 0; i < Sprites.Value.Count; i++) {
					Bitmap frame = new Bitmap(Sprites.Value[i]);
					TotalWidth += frame.Width;
					if(TotalHeight < frame.Height) TotalHeight = frame.Height;
					Frames[i] = frame;
				}
				System.Drawing.Imaging.ImageFormat _format = System.Drawing.Imaging.ImageFormat.Png;
				switch(format) {
					case "PNG":
						_format = System.Drawing.Imaging.ImageFormat.Png;
						break;
					case "JPG":
						_format = System.Drawing.Imaging.ImageFormat.Jpeg;
						break;
				}
				Bitmap result = new Bitmap(TotalWidth, TotalHeight);
				try {
					using(Graphics g = Graphics.FromImage(result)) {
						int currentPos = 0;
						for(int i = 0; i < Frames.Length; i++) {
							g.DrawImage(Frames[i], currentPos, 0);
							currentPos += Frames[i].Width;
						}
						result.Save(Sprites.Value.OutputFile,_format);
						Console.WriteLine("{0} done",Sprites.Value.OutputFile);
					}
				}
				catch(Exception e) {
					Console.WriteLine("Something gone wrong: {0}" + e.Message);
					return;
				}
				result.Dispose();
			}
		}
		
		// Check all paths
		private bool CheckFiles() {
			// Checking equality of output files and input directories
			if(sources.Length != destinations.Length) {
				Console.WriteLine("Wrong source/destination parameters");
				return false;
			}
			for(int i = 0; i < sources.Length; i++) {
				// Checking input directories exists
				if(!Directory.Exists(sources[i])) {
					Console.WriteLine("{0} path not exists",sources[i]);
					return false;
				} else {
					//Checking files in existing directories;
					if(Directory.GetFiles(sources[i],"*."+format.ToLower()).Length < 2) {
						Console.WriteLine("There are no or less than 2 files in directory {0}",sources[i]);
						return false;
					}
				}
				//Checking output files for existing
				if(File.Exists(destinations[i]) && !overwrite) {
					Console.WriteLine("{0} already exists. Use --overwrite");
					return false;
				}
			}
			return true;
		}
	}
}