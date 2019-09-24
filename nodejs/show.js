var contentMap = require('./content');

function showPage(response, pathName){

    if (pathName === '/favicon.ico') {
        response.writeHead(200, {'Content-Type': 'image/x-icon'} );
        return response.end();
    }

    if(contentMap.contentMap[pathName]){
        response.writeHead(200, {'Content-Type': 'text/html'});
        response.write(contentMap.contentMap[pathName]);
        response.end();
    }else {
        response.writeHead(404, {'Content-Type': 'text/html'});
        response.write('404 Page not found');
        response.end();
    }
}

exports.showPage = showPage;