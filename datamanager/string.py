"""Very basic string manipulation functions."""

def toArray(stri):
    """Converts the given string into an array of strings."""
    a = []
    for x in range(len(stri)):
        a.append(stri[x])
    return a


def fromArray(arr, separator=""):
    """Creates a string from the given array."""
    string = ""
    for el in arr:string += str(el) + separator
    return string


def toRange(stri):
    """Returns the range object for this string."""
    return range(len(stri))


def remove_characters(stri, chars):
    """Removes all the given characters from the given string."""
    new_str = ""
    for char in toArray(stri):
        if not char in chars:new_str += char          
    return new_str


def remove_characters_from(stri, start, end):
    """Removes all the characters of a string starting at 'start' and ending at 'end' (indices). Start is included and end is excluded."""   
    end = len(stri) if end == None else end
    new_str = ""
    for x in toRange(stri):
        if x < start or x >= end:new_str += stri[x]
    return new_str


def remove_characters_from_char(stri, start, end=None):
    """Removes all the characters of a string between the characters 'start' and 'end'. Start is included and end is excluded."""
    new_str = ""
    add_c = True
    for x in toRange(stri):
        if stri[x] == start:add_c = False
        if stri[x] == end  :add_c = True
        if add_c:new_str += stri[x]
    return new_str


def list_separator(stri, separator="-"):
    """eg: list_separator('2020-06-13', '-') => [2020,06,13] """
    current_string = ""
    arr = []

    for char in toArray(stri):
        if char == separator and not is_empty(current_string):
            arr.append(current_string)
            current_string = ""
        else: current_string += char
    arr.append(current_string.rstrip("\n"))
    return arr


def reverse(stri):
    """Reverses the given string."""
    string_array = toArray(stri)
    string_array.reverse()
    return fromArray(string_array)


def remove_index(stri, indices=[0]):
    """Removes the given indeces from the string."""
    new_string = ""
    for x in toRange(stri):
        if not x in indices:new_string+=stri[x]
    return new_string


def remove_last(stri):
    """Removes the last element from the given string."""
    return remove_index(stri, [len(stri)-1])


def remove_first(stri):
    """Removes the first element from the given string."""
    return remove_index(stri,0)


def get_last_index(stri):
    """Returns the index of the last element of the string."""
    return len(stri) - 1


def map_string(stri, func):
    """Constructs a new string from the original string called the function. The function should have as its arguments the character and the index."""
    new_string = ""
    for x in toRange(stri):
        char = stri[x]
        new_string += func(char, x)
    return new_string


def is_empty(stri):
    """Returns true if the given string does not have any characters or if all the characters are spaces."""
    a = True
    for char in stri: a = a and char == " "
    return a