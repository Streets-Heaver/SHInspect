[Main Page](https://github.com/Streets-Heaver/SHInspect/blob/main/README.md)

## Searching for elements

There are multiple ways to search for elements. These methods are intended to help improve workflow. You can search for elements so long as there is an active window that has been added to your saved windows/windows to display. (see [here](https://github.com/Streets-Heaver/SHInspect/blob/main/SHInspect/Documentation/Settings.md))

### Inspecting elements

You can grab an element directly below the cursor by holding CTRL + ALT. This will highlight the element in its inspection colour and select it. Occasionally you may need to manually traverse the tree if a parent element or child element was picked up.

### Using the search function

A search function is also available. The search function searches within the currently selected item. Therefore to search within your entire application you would need to select the window/root element first. 

![image](/SHInspect/Documentation/images/Search.PNG)

You can select a search type from the drop down. Currently available are 'AutomationId','Name','ClassName','ControlType' and 'XPath'. Once selected simply type in the search box and press enter. The search can return multiple results and therefore arrows are available to change between the results. This will select each element.

### Traversing the visual tree

The tree can be traversed by using the collapse/expand buttons on each item. There are also useful context menu options and shortcuts to make tree traversal easier. (see [here](https://github.com/Streets-Heaver/SHInspect/blob/main/SHInspect/Documentation/ContextMenuShortcuts.md))