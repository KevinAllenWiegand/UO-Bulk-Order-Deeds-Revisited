var appRouter = function(router, fs, path, mkdirRecursive, rootDirectory, moment, _, exec, uuid, sanitize) {	
	router.get("/UOBulkOrderDeeds/Status", function(request, response) {
		response.status(200).json({ status: "OK" });
	});
	
	router.get("/UOBulkOrderDeeds/CurrentVersion", function(request, response) {
		var action = request.query["action"] || "version";
			
		if (action === "version") {
			var version = fs.readFileSync("Files\\version.txt", "utf8");
		
			response.status(200).json({ status: "OK", version: version });
		} else if (action === "get") {
			var options = {
				root:  rootDirectory + "\\Files\\"
			};
			
			response.sendFile("UOBulkOrderDeedsRevisited.msi", options);
		} else {
			response.status(400).json({ message: "Unknown action requested." });
		}
	});
	
	router.post("/UOBulkOrderDeeds/BulkOrderDeedReport", function(request, response) {
		var folder = rootDirectory + "\\BulkOrderDeedReports\\";
		
		if (!fs.existsSync(folder)) {
			try {
				mkdirRecursive.mkdirSync(folder);
			} catch (ex) {
				console.log("Error creating report folder:  " + ex.message);
			}
		}
			
		var filename = request.headers["x-file-name"] || "unknown";
		
		filename = sanitize(filename) || "unknown";
		fs.writeFile(folder + filename + "." + uuid(), request.body);
		
		response.status(200).json({ status: "OK" });
	});
	
	router.post("/UOBulkOrderDeeds/Feedback", function(request, response) {	
		var folder = rootDirectory + "\\BulkOrderDeedFeedback\\";
		
		if (!fs.existsSync(folder)) {
			try {
				mkdirRecursive.mkdirSync(folder);
			} catch (ex) {
				console.log("Error creating feedback folder:  " + ex.message);
			}
		}
		
		fs.writeFile(folder + uuid() + ".json", JSON.stringify(request.body));
		
		response.status(200).json({ status: "OK" });
	});
}
 
module.exports = appRouter;