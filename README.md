# What could potentially go wrong?

[![.NET](https://github.com/cosh/whatcouldpotentiallygowrong/actions/workflows/buildtest.yml/badge.svg?branch=main)](https://github.com/cosh/whatcouldpotentiallygowrong/actions/workflows/buildtest.yml)

This is a project to do some research on how to protect from mallicous code injected during runtime.

## Example


curl request:
```
curl -X 'POST' \
  'https://localhost:49161/Execute' \
  -H 'accept: application/json' \
  -H 'Content-Type: application/json' \
  -d '{
  "code": "return _ => _ * 2;",
  "input": 10
}'
```

response:
```json
{
  "spec": {
    "code": "return _ => _ * 2;",
    "input": 10
  },
  "result": 20
}
```