//using Microsoft.AspNetCore.Mvc;
//using SkiaSharp;

//namespace YourWebApi.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class ImageController : ControllerBase
//    {
//        [HttpGet]
//        public IActionResult Get(int width = 200, int height = 100, string shape = "rectangle")
//        {
//            // Criar uma imagem usando SkiaSharp
//            using (var bitmap = new SKBitmap(width, height))
//            {
//                using (var canvas = new SKCanvas(bitmap))
//                {
//                    canvas.Clear(SKColors.White);

//                    // Desenhar com base nos parâmetros fornecidos
//                    switch (shape.ToLower())
//                    {
//                        case "rectangle":
//                            DrawRectangle(canvas);
//                            break;
//                        case "circle":
//                            DrawCircle(canvas);
//                            break;
//                        case "test":
//                            Test(canvas);
//                            break;
//                        // Adicione mais casos para outros tipos de formas ou figuras geométricas
//                        default:
//                            DrawRectangle(canvas);
//                            break;
//                    }
//                }

//                // Salvar a imagem em um formato suportado (por exemplo, PNG)
//                using (var image = SKImage.FromBitmap(bitmap))
//                {
//                    using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
//                    {
//                        // Converter a imagem para bytes
//                        var bytes = data.ToArray();
//                        // Retornar os bytes da imagem como resposta
//                        return File(bytes, "image/png");
//                    }
//                }
//            }
//        }

//        private void DrawRectangle(SKCanvas canvas)
//        {
//            // Desenhar um retângulo vermelho
//            using (var paint = new SKPaint())
//            {
//                paint.Color = SKColors.Red;
//                canvas.DrawRect(new SKRect(50, 25, 150, 75), paint);
//            }
//        }

//        private void DrawCircle(SKCanvas canvas)
//        {
//            // Desenhar um círculo azul
//            using (var paint = new SKPaint())
//            {
//                paint.Color = SKColors.Blue;
//                canvas.DrawCircle(100, 50, 40, paint);
//            }
//        }

//        private void Test(SKCanvas canvas)
//        {
//            // Definir o centro e raio do círculo
//            float centerX = canvas.LocalClipBounds.Width / 2;
//            float centerY = canvas.LocalClipBounds.Height / 2;
//            float radius = Math.Min(centerX, centerY) * 0.8f; // Ajustar o raio conforme necessário

//            // Criar os vértices para representar um círculo
//            SKPoint[] points = new SKPoint[360];
//            for (int angle = 0; angle < 360; angle++)
//            {
//                float radians = angle * (float)Math.PI / 180.0f;
//                float x = centerX + radius * (float)Math.Cos(radians);
//                float y = centerY + radius * (float)Math.Sin(radians);
//                points[angle] = new SKPoint(x, y);
//            }

//            // Criar os índices para desenhar os triângulos que formarão o círculo
//            ushort[] indices = new ushort[360];
//            for (ushort i = 0; i < 360; i++)
//            {
//                indices[i] = i;
//            }

//            // Criar os vértices e desenhar o círculo azul
//            SKVertices vertices = SKVertices.CreateCopy(SKVertexMode.TriangleFan, points, null, null, indices);
//            using (var paint = new SKPaint())
//            {
//                paint.Color = SKColors.Blue;
//                canvas.DrawVertices(vertices, SKBlendMode.SrcOver, paint);
//            }
//        }
//    }
//}
