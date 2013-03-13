# TextWrapper

Just some crappy code I haacked (get it? get it?) together to format a text 
document (or set of documents) to wrap at a max column width. At the moment, 
it’s pretty crappy. It does just enough for what I needed at the moment. But I 
will accept improvements via pull requests!

## Usage

Convert a document to wrap at 80 columns.

```
TextWrapper .\README.md
```

Results in a `README.md.out` file.

Convert a document to wrap at 78 columns.

```
TextWrapper .\README.md 78
```

You can even pass in a directory to convert every `*.txt` and `*.md` file (all 
of this should be configurable).

```
TextWrapper .\my-avante-garde-poetry
```


## Caveats
There’s like no error checking or anything yet. I wrote this as a quick and 
dirty thing and it’s definitely dirty.

