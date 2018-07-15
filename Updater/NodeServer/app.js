var port = 8010;
var express = require("express");
var bodyParser = require("body-parser");
var mkdirRecursive = require("mkdir-recursive");
var path = require("path");
var _ = require("underscore");
var rootDirectory = path.dirname(require.main.filename);
var app = express();
var fs = require("fs");
var router = express.Router();
var moment = require("moment");
var exec = require("child_process").exec;
var uuid = require("uuid/v4");
var sanitize = require("sanitize-filename");
var routes = require("./routes/routes.js")(router, fs, path, mkdirRecursive, rootDirectory, moment, _, exec, uuid, sanitize);

app.use(bodyParser.json({ limit: "1mb" }));
app.use(bodyParser.urlencoded({ extended: true, limit: "50mb" }));
app.use(bodyParser.raw({ inflate: true, type: "application/octet-stream", limit: "1mb" }));

app.use(function(request, response, next) {
	response.setHeader('Access-Control-Allow-Origin', '*');
	response.setHeader("Access-Control-Allow-Methods", "GET,HEAD,OPTIONS,POST,PUT,DELETE");
    response.setHeader("Access-Control-Allow-Headers", "Access-Control-Allow-Headers, Origin,Accept, X-Requested-With, Content-Type, Access-Control-Request-Method, Access-Control-Request-Headers");
	next();
});

app.use(function(error, request, response, next) {
	var errors = [];
	var statusCode = 500;
	
	errors.push({message: error.message})
	
	if (error.name === "SyntaxError") {
		statusCode = 400
	}
	
	response.status(statusCode).json({errors: errors});
})

app.use("/", router);

process.on('uncaughtException', function(error) {
	console.log("***********************************\r\n");
	console.log(error + "\r\n");
	console.log("***********************************\r\n");
});

var server = app.listen(port, function() {	
	console.log("[" + moment().format("MM-DD-YYYY HH:mm:ss") + "] Listening on port %s...", server.address().port);
});