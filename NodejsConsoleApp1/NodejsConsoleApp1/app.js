"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const Parent_1 = require("./Parent");
console.log('Hello world');
var parent = new Parent_1.Parent();
parent.Print("new Parent");
process.stdin.setRawMode(true);
process.stdin.resume();
process.stdin.on('data', process.exit.bind(process, 0));
//# sourceMappingURL=app.js.map