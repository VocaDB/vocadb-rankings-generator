# VocaDB rankings generator

Command line application that generates Vocaloid/UTAU song rankings based on [VocaDB](http://vocadb.net).

The application generates a static .html file based on a [Razor](http://www.asp.net/web-pages/overview/getting-started/introducing-razor-syntax-c) template. 
By default the rankings are generated for the previous week, but the start date can be set manually as well.

[Example here](http://vocaloid.eu/vocaloid/rankings/weekly/2016-18.html).

[.NET framework 4.5.2](https://www.microsoft.com/en-us/download/details.aspx?id=49981) (or newer) is needed. 
No other dependencies.