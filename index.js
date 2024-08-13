const express = require('express');
const bodyParser = require('body-parser');
const app = express();
const port = 3000;

// Middleware to parse incoming text content
app.use(bodyParser.text());

app.post('/receive-content', (req, res) => {
    const content = req.body;
    
    if (content) {
        console.log(`Received content: ${content}`);
        res.sendStatus(200);
    } else {
        console.log('No content received');
        res.sendStatus(400); // Bad request if content is missing
    }
});

app.listen(port, () => {
    console.log(`Server is running on http://localhost:${port}`);
});
