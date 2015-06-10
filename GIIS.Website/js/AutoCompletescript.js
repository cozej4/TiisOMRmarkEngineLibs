function pageLoad(sender, args) {
    ClientPopulated(sender, args);
}

function ClientPopulated(source, eventArgs)
{
    if (source._currentPrefix != null)
    {
        var list = source.get_completionList();
        var search = source._currentPrefix.toLowerCase();
        for (var i = 0; i < list.childNodes.length; i++)
        {
            var text = list.childNodes[i].innerHTML; 
            var index = text.toLowerCase().indexOf(search);
            if (index != -1)
            {
                var value = text.substring(0, index);
                value += '<span class="AutoComplete_ListItemHiliteText">';
                value += text.substr(index, search.length);
                value += '</span>';
                value += text.substring(index + search.length);
                list.childNodes[i].innerHTML = value;
            }
        }
    }
}