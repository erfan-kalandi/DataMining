# DataMining Project 

The project includes a memory section that initially reads all the pages, separates their lines, and puts them all in a list for each page. Then it combines the created lists into a new list called "PapersLines." It also creates a dictionary for genres, where the key is the genre name, and the value is a list of words found in that genre. This is done by reading each file separately, splitting the words, and putting them in a list. This way, there is no need to read separate files for each section.

Part 1:
This part takes a desired word and the minimum number of occurrences as input and returns the number of pages that have the specified word with the given number of repetitions. It works by taking the list of lines for each page from the initial list and returns the line number where the word is first found. In the worst case, it checks the entire paragraph starting from the first line where the word is found. After finding the desired word in the specified number of occurrences on a page, it adds the page to the answer list. Finally, it returns the list of pages.

Part 2:
This part takes a desired genre as input and returns the pages that have that genre. It works by taking the list of lines for each page from the initial list and checks some words from the first line of each paragraph to see if they exist in the list of words in the dictionary with the key matching the desired genre. If a match is found, it adds the page number to the answer list. Finally, it returns the list of pages.

Part 3:
This part uses a helper method called "findword." By providing the desired genre and the target page, it returns the words of the genre found in that page. Then, using the dictionary, it determines the frequency of each word in that paragraph. After that, for each page, it finds the desired genre's words and compares them with the words in the reference page. If the word is present in the second page, it calculates its frequency and takes the minimum between the frequency of that word in the reference page and the compared page. It adds this value to the count of common words and, if it exceeds the desired count, adds the page to the adjacent list. Finally, it returns the list of adjacent pages.

Part 4:
It utilizes Part 3 and, since no specific genre is given, it executes Part 3 for all genres and combines the results.

Part 5:
This section uses a helper method called "CheckAdjacency." By providing the reference page and the page to compare with the desired genre and the minimum number of repetitions for adjacency, it checks if two pages in that genre are adjacent or not. The functionality is similar to Part 4, but instead of adding it to the adjacency list, it returns a boolean. In the main function of Part 5, if two pages are adjacent, it increments the counter of common genres between the two pages. This process is performed for all genres, and if the counter of common genres between two pages equals the desired value, it adds the page to the adjacency list. Finally, it returns the adjacency list.

Part 6:
In this part, for each page, it finds the desired genre's words and using the frequency dictionary, it determines the frequency of each word in that page. Then, it compares it with the remaining pages, and if the desired word count is shared between two pages, it considers them adjacent. Using a queue, it finds the adjacent page until all pages with the desired genre are checked. Finally, it adds the book and returns all possible books.

Part 7:
It utilizes Part 6 and, after finding all the books, it uses a helper function to find the books that have the desired count of the specified word and returns them.
