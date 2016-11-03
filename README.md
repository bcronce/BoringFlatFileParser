## BoringFlatFileParser
- Async
- Minimize garbage collection in Gen2

## History
I have a situation where existing popular parsing libraries and accessing fields as strings is causing large amounts of strings to be quickly promoted to Gen2. My idea is to write datarecords into char buffers where the programmer is free to defer converting them into strings at some later time.