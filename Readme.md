## BadgeMaker v1.0
### A command line utility to generate status badges.
----

Thanks to [Shields.io](https://shields.io/) for making their online utility for badge generation.  This utility uses the same named colors and a basic svg format to generate svg status badges offline.

[Avalonia](https://avaloniaui.net/) is used to measure the text widths.
[CommandLineUtils](https://github.com/natemcmaster/CommandLineUtils) is used for parsing args.

```
Usage: BadgeMaker [options]

Options:
  -?|-h|--help               Show help information.
  -l|--left <LEFT_TEXT>      Text for the left side of the badge.

  -r|--right <RIGHT_TEXT>    Text for the right side of the badge.

  -o|--output <OUTPUT_PATH>  Full path for the output of the badge svg file.

  -c|--color <COLOR>         Hex Color for the right side of the badge.

                             Named colors:
                                blue: '#007ec6' (Default)
                                brightgreen: '#4c1'
                                green: '#97ca00'
                                yellow: '#dfb317'
                                yellowgreen: '#a4a61d'
                                orange: '#fe7d37'
                                red: '#e05d44'
                                grey: '#555'
                                lightgrey: '#9f9f9f'

```
