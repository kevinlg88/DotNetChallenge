using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebApp.Data;
using WebApp.Models;
using Microsoft.Data.Sqlite;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly string _connectionString;


        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpPost]
        public async Task<IActionResult> DepartamentosSalarios()
        {
            ViewBag.resultOne = true;
            var query = @"
            SELECT 
                d.Nome AS Departamento, 
                p.Nome AS Pessoa, 
                p.Salario
            FROM 
                Pessoas p
            JOIN 
                Departamentos d ON p.DeptId = d.Id
            WHERE 
                p.Salario = (
                    SELECT MAX(Salario) 
                    FROM Pessoas 
                    WHERE DeptId = p.DeptId
                )
            ORDER BY 
                p.Salario DESC;";

            using (var connection = new SqliteConnection(_connectionString))
            {
                try
                {
                    var data = (await connection.QueryAsync<DepartamentoPessoaViewModel>(query)).ToList();
                    return View("Index",data);
                }
                catch (SqliteException ex)
                {
                    // Log or handle the exception as needed
                    return StatusCode(500, "Internal server error");
                }
            }
        }

        [HttpPost]
        public IActionResult Arvore()
        {
            int[] array1 = { 3, 2, 1, 6, 0, 5 };
            var tree1 = BuildTree(array1);

            int[] array2 = {7, 5, 13, 9, 1, 6, 4};
            var tree2 = BuildTree(array2);

            ViewBag.Arvore1 = tree1;
            ViewBag.Arvore2 = tree2;

            return View("Index");
        }

        private int[][] BuildTree(int[] arr)
        {
            if (arr.Length == 0) return new int[0][];

            int maxIndex = Array.IndexOf(arr, arr.Max());
            int root = arr[maxIndex];

            // Sub-arrays para os galhos esquerdo e direito
            int[] leftBranch = arr.Take(maxIndex).OrderByDescending(x => x).ToArray();
            int[] rightBranch = arr.Skip(maxIndex + 1).OrderByDescending(x => x).ToArray();

            return new int[][] { new int[] { root }, leftBranch, rightBranch };
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
