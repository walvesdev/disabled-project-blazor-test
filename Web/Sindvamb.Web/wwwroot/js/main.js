function redirect(path) {
    return window.location = path;
}

window.downloadFileFromStream = async (fileName, contentStreamReference) => {
    const arrayBuffer = await contentStreamReference.arrayBuffer();
    const blob = new Blob([arrayBuffer]);
    const url = URL.createObjectURL(blob);
    const anchorElement = document.createElement('a');
    anchorElement.href = url;
    anchorElement.download = fileName ?? '';
    anchorElement.click();
    anchorElement.remove();
    URL.revokeObjectURL(url);
}

window.revokeObjectURL = async (url) => {
    URL.revokeObjectURL(url);
}

window.urlFileFromStream = async (fileName, contentStreamReference) => {
    const arrayBuffer = await contentStreamReference.arrayBuffer();
    const blob = new Blob([arrayBuffer]);

    return URL.createObjectURL(blob);;
}

window.BlazortJavascriptOnClick = function (id) {
    console.log(id)
    document.getElementById(id).click();
}