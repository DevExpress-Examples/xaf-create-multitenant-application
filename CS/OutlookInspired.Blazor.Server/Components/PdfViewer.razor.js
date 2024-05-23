export async function addPdfLoadListener(element, base64Pdf, toolbar) {
    const pdfEmbed = element;
    if (pdfEmbed) {
        const newEmbed = pdfEmbed.cloneNode();
        const loadHandler = function () {
            newEmbed.removeEventListener("load", loadHandler);
        };
        newEmbed.addEventListener("load", loadHandler);
        pdfEmbed.parentNode.replaceChild(newEmbed, pdfEmbed);
        newEmbed.setAttribute("src", `data:application/pdf;base64,${base64Pdf}#toolbar=${toolbar}`);
    }
}
