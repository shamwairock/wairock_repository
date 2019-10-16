import { Parent } from "./Parent";

console.log('Hello world');

var parent = new Parent();
parent.Print("new Parent");

process.stdin.setRawMode(true);
process.stdin.resume();
process.stdin.on('data', process.exit.bind(process, 0));