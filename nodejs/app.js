var http = require('http');
var url = require('url');
var show = require('./show');

http.createServer(onRequest).listen(8889);
console.log('Server has started');

function onRequest(request, response){
  var pathName = url.parse(request.url).pathname;
  show.showPage(response, pathName);
}






