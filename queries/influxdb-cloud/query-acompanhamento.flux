# Acompanhamento de pessoas com algum registro temperatura alta
# dentro do intervalo de tempo

import "array"

list = from(bucket: "datalake_cloud")
  |> range(start: v.timeRangeStart, stop: v.timeRangeStop)
  |> filter(fn: (r) => r["_measurement"] == "temperature")
  |> filter(fn: (r) => r["_value"] >= 38)
  |> group(columns: ["personId"])
  |> first()
  |> map(fn: (r) => ({ personId: r["personId"] }))
  |> group()
  |> findColumn(
    column: "personId",
    fn: (key) => true
  )

from(bucket: "datalake_cloud")
  |> range(start: v.timeRangeStart, stop: v.timeRangeStop)
  |> filter(fn: (r) => r["_measurement"] == "temperature")
  |> filter(fn: (r) => contains(value: r["personId"], set: list))