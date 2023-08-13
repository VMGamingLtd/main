db = connect( 'mongodb://localhost/gaos' );
console.log("INFO: Dropping mongo database 'gaos'")
db.dropDatabase()
