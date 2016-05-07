# UnMagicalStrings

Make your resources type safe and IntelliSense friendly

## Getting Started

### Prerequisities

.NETFramework v4.5+

### Installing

Just copy `ums.exe` to a folder in your path

### Usage

* `-t` or `-target` or first argument: the output file name or location (e.g. MyResources.cs). If not specified, the console stdout is used.
* `-s` or `-source`: the base directory to scan for files. If not specified, the current working directory is used.
* `-n` or `-namespace`: the namespace for the resources class.
* `-i` or `-include`: the patterns of files and folders to should be included relative to the base directory.
* `-e` or `-exclude`: the patterns of files and folders to should be excluded relative to the base directory.

The pattern matching style can be exact or partial, using `*` syntax (e.g. `file.ext`, `folder*`, `folder\subfolder*`, `*file*`, `*.ext`)

### Samples

Sample folder structure:

* description.txt
* images
	* image.bmp
	* image.gif
* styles
     * main.css
     * colors.css
     * headers.css
* scripts
    * main.js

Scan all files and folders in the current location:

```
C:\demo>ums.exe
```

```csharp
public partial class Resources
{
    public const string description_txt = @"description.txt";
    public const string imagesFolder = @"images\";
    public partial class images
    {
        public const string image_bmp = @"images\image.bmp";
        public const string image_gif = @"images\image.gif";
    }
    public const string scriptsFolder = @"scripts\";
    public partial class scripts
    {
        public const string main_js = @"scripts\main.js";
    }
    public const string stylesFolder = @"styles\";
    public partial class styles
    {
        public const string colors_css = @"styles\colors.css";
        public const string headers_css = @"styles\headers.css";
        public const string main_css = @"styles\main.css";
    }
}
```

Specify `target`, `namespace`, `include` and `exclude` patterns:

```
C:\demo>ums.exe MyResources -namespace MyNamespace -include images* -exclude *.gif
```

```csharp
namespace MyNamespace
{
    public partial class Resources
    {
        public const string imagesFolder = @"images\";
        public partial class images
        {
            public const string image_bmp = @"images\image.bmp";
        }
    }
}
```

## Questions, Issues and Contributing
* Have a question? [Ask at Stackoverflow](http://stackoverflow.com/questions/tagged/UnMagicStrings)
* Found an problem? [Create an issue](https://github.com/marcosbozzani/unmagicstrings/issues)
* Want to contribute? [Send a pull request](https://github.com/marcosbozzani/unmagicstrings/pulls)


## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details

## Acknowledgments

* Hat tip to anyone who's code was used
* [Vapour in the Alley](http://stackoverflow.com/users/158821/vapour-in-the-alley) for the command line arguments parser
* [Adriano Repetti](http://stackoverflow.com/users/1207195/adriano-repetti) for the PathAddBackslash method

