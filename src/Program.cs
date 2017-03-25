/**
 * Author: Andrej Bestuzhev
 * E-mail: andrej.bestuzhev@gmail.com
 * Web: http://poest.net
 * Created: 31.03.2015
 */
     

using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SpriteUtils {

	class Program {
		static void Main(string[] args) {
		
			if(args.Length == 0) {
				Console.WriteLine("No arguments. Type -h for help");
				return;
			}
			int platform = (int)Environment.OSVersion.Platform;
			int[] UnixPlatform = new int[] { 4,6,128 };
			
			if(args.Length == 1 && args[0] == "-h") {
				string exec;
				if(Array.IndexOf(UnixPlatform,platform) != -1)	exec = "./sfitter";
				else exec = "sfitter.exe";
				Console.WriteLine("Usage: {0} --from=<inputDir> [--format=<format>] [--mirror] [--reverse] [--to=<outputFile>]",exec);
				Console.WriteLine("\n--format : Format of output image (JPG or PNG).");
				Console.WriteLine("--from : path to directory with your sprites.");
				Console.WriteLine("--to : output file.");
				Console.WriteLine("--overwrite : overwrite existing files\n");
                Console.WriteLine("\nAdditional options:");
                Console.WriteLine("--reverse : write slides in reverse order");
                Console.WriteLine("--mirror : flip each slide horizontally");
				return;
			}
			
			// possible arguments
			
			string[] allowed = { "--format", "--from", "--to", "--overwrite", "--mirror", "--reverse" };
			bool Error = false;
			string Format = "PNG";
			string From = "", To = "";
			bool OverWrite = false;
            bool Mirror = false;
            bool ReverseOrder = false;
			
			foreach(string s in args) {
				if (Error) break;
				string[] prepare = s.Split(new string[] { "=" }, StringSplitOptions.None);
				if (allowed.Contains(prepare[0])) {
					switch(prepare[0]) {
						case "--format":
							if(prepare[1] == "JPG" || prepare[1] == "PNG") Format = prepare[1];
							else {
								Error = true;
								Console.WriteLine("Error: {0} - unrecognized format", prepare[0]);
								break;
							}
							break;
						case "--from":
							From = prepare[1];
							break;
						case "--to":
							To = prepare[1];
							break;
						case "--overwrite":
							OverWrite = true;
							break;
                        case "--reverse":
                            ReverseOrder = true;
                            break;
                        case "--mirror":
                            Mirror = true;
                            break;
                    }
				}
			}
			if(From == "") {
				Console.WriteLine("No input path defined. Use ./ for current path.");
				return;
			}
			if(To == "") To = "out."+Format.ToLower();
			SpriteFitter spriteFitter = new SpriteFitter(new string[] { From }, new string[] { To }, Format, OverWrite, ReverseOrder, Mirror);
			spriteFitter.Process();
		}
	}	
}

