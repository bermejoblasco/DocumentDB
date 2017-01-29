﻿function simple(prefix) {

    var collection = getContext().getCollection();



    // Query documents and take 1st item.

    var isAccepted = collection.queryDocuments(

        collection.getSelfLink(),

        'SELECT * FROM root r',

        function (err, feed, options) {

            if (err) throw err;



            // Check the feed and if it's empty, set the body to 'no docs found',

            // Otherwise just take 1st element from the feed.

            if (!feed || !feed.length) getContext().getResponse().setBody("no docs found");

            else getContext().getResponse().setBody(prefix + JSON.stringify(feed[0]));

        });



    if (!isAccepted) throw new Error("The query wasn't accepted by the server. Try again/use continuation token between API and script.");

}