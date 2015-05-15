// From: http://the360view.typepad.com/blog/2015/03/lab4-view-and-data-api-web-intro-js.html

var ViewerEmbed = {

    // Initialize the viewer, which includes setting a callback function
    // to load the model on success. 
    // Arguments: 
    // 1) getToken - callback function to get a valid token
    // 2) docUrn - urn of a uploaded model
    // 3) viewerElement - a portion of html document to embed a viewer. 
    //
    initialize: function (getToken, docUrn, viewerElement) {

        // Options: 
        // env is used in to set environment variables. 
        // getAccessToken and refreshToken are called from
        // Autodesk.Viewing.Initializer to get valid access token

        var options = {
            'env': 'AutodeskProduction',
            'getAccessToken': getToken,
            'refreshToken': getToken,
        };

        // Create a viewer with basic default set of UI.    
        var viewer = new Autodesk.Viewing.Private.GuiViewer3D(viewerElement, {
            extensions: ['BasicExtension']
        });

        // Older call used in earlier documentation does not display toolbar
        //var viewer = new Autodesk.Viewing.Viewer3D(viewerElement, {});

        // Initialize the viewer with the options.
        // Set the callback to actually load a model for viewing. 
        Autodesk.Viewing.Initializer(options, function () { // onSuccessCallback 
            viewer.initialize();
            ViewerEmbed.loadDocument(viewer, docUrn); // defined below. 
        });
    },

    // Load a document with a given id (=urn) in the viewer.

    loadDocument: function (viewer, documentId) {

        // Find the first 3d geometry and load it.
        Autodesk.Viewing.Document.load(documentId, // arg1: urn 
            function (doc) { // arg2: onLoadCallback
                var geometryItems = [];
                geometryItems = Autodesk.Viewing.Document.getSubItemsWithProperties(
                    doc.getRootItem(), // item - document node to begin searching from 
                    { // properties to search for 
                        'type': 'geometry', // geometry/view/resource/folder/, etc.
                        'role': '3d'  // 2d/3d/thumbnail/, etc.   
                    },
                    true); // recursive searcn, true/false 

                if (geometryItems.length > 0) {
                    // Finally, load the model 
                    viewer.load(doc.getViewablePath(geometryItems[0]));
                }
            },
            function (errorMsg) { // arg3: onErrorCallback
                alert("Load Error: " + errorMsg);
            }
        );
    }
}